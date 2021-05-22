using CapstoneProject1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneProject1.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(FormCollection form)
        {
            int _firstLine;
            double _timeStep;
            double maxp = 0;
            double maxn = 0;
            // Harita için datalar
            List<Koordinat> koordinatlar = new List<Koordinat>();
            //

            try
            {
                _firstLine = Convert.ToInt16(form["FirstLine"]);
                _timeStep = Convert.ToDouble(form["TimeStep"]);
            }
            catch (Exception e)
            {
                for (int i = 0; i < 5; i++)
                {
                    Koordinat boskoordinat = new Koordinat();
                    boskoordinat.dosyaAdi = "";
                    boskoordinat.latitudeE = "0.0000";
                    boskoordinat.longitudeE = "0.0000";
                    boskoordinat.latitudeS = "0.0000";
                    boskoordinat.longitudeS = "0.0000";
                    koordinatlar.Add(boskoordinat);
                }
                ViewBag.Message = "You entered incomplete or incorrect data!";
                return View(koordinatlar);
            }
            string _fileType = form["FileType"].ToString();
            List<string> dosyaYolu = new List<string>();

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fi = new FileInfo(file.FileName);
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                        file.SaveAs(path);
                        dosyaYolu.Add(path.ToString());
                    }
                }
            }

            if (dosyaYolu.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Koordinat boskoordinat = new Koordinat();
                    boskoordinat.dosyaAdi = "";
                    boskoordinat.latitudeE = "0.0000";
                    boskoordinat.longitudeE = "0.0000";
                    boskoordinat.latitudeS = "0.0000";
                    boskoordinat.longitudeS = "0.0000";
                    koordinatlar.Add(boskoordinat);
                }
                ViewBag.Message = "You have not selected any files!";
                return View(koordinatlar);
            }
            else if (dosyaYolu.Count > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Koordinat boskoordinat = new Koordinat();
                    boskoordinat.dosyaAdi = "";
                    boskoordinat.latitudeE = "0.0000";
                    boskoordinat.longitudeE = "0.0000";
                    boskoordinat.latitudeS = "0.0000";
                    boskoordinat.longitudeS = "0.0000";
                    koordinatlar.Add(boskoordinat);
                }
                ViewBag.Message = "You can choose up to 5 files!";
                return View(koordinatlar);
            }

            ViewBag.DosyaSayisi = Request.Files.Count.ToString();

            // Grafik için datalar
            List<DataPoint> dataPoints1 = new List<DataPoint>();    // Acceleration Chart 1
            List<DataPoint> dataPoints2 = new List<DataPoint>();    // Acceleration Chart 2
            List<DataPoint> dataPoints3 = new List<DataPoint>();    // Acceleration Chart 3
            List<DataPoint> dataPoints4 = new List<DataPoint>();    // Acceleration Chart 4
            List<DataPoint> dataPoints5 = new List<DataPoint>();    // Acceleration Chart 5
            List<DataPoint> dataPoints6 = new List<DataPoint>();    // Velocity Chart 1
            List<DataPoint> dataPoints7 = new List<DataPoint>();    // Velocity Chart 2
            List<DataPoint> dataPoints8 = new List<DataPoint>();    // Velocity Chart 3
            List<DataPoint> dataPoints9 = new List<DataPoint>();    // Velocity Chart 4
            List<DataPoint> dataPoints10 = new List<DataPoint>();    // Velocity Chart 5
            List<DataPoint> dataPoints11 = new List<DataPoint>();    // Displacement Chart 1
            List<DataPoint> dataPoints12 = new List<DataPoint>();    // Displacement Chart 2
            List<DataPoint> dataPoints13 = new List<DataPoint>();    // Displacement Chart 3
            List<DataPoint> dataPoints14 = new List<DataPoint>();    // Displacement Chart 4
            List<DataPoint> dataPoints15 = new List<DataPoint>();    // Displacement Chart 5
            //

            for (int i = 0; i < Request.Files.Count; i++)
            {
                Koordinat koordinat = new Koordinat();

                double timeScale = 0.0;

                double preAcceleration = 0.0, preVelocity = 0.0, preDisplacement = 0.0;

                if (_fileType.Equals("Single"))
                {
                    StreamReader lines = null;
                    try
                    {
                        var file = Request.Files[i];
                        koordinat.dosyaAdi = file.FileName;
                        lines = System.IO.File.OpenText(dosyaYolu[i]);
                        String datalar = null;
                        int dataCount = 0;
                        while ((datalar = lines.ReadLine()) != null)
                        {
                            dataCount++;
                            if (dataCount == 5)
                            {
                                String[] dg = datalar.Split(' ');
                                koordinat.latitudeE = dg[1].Replace(',', '.');
                            }
                            else if (dataCount == 6)
                            {
                                String[] dg = datalar.Split(' ');
                                koordinat.longitudeE = dg[1].Replace(',', '.');
                            }
                            else if (dataCount == 17)
                            {
                                String[] dg = datalar.Split(' ');
                                koordinat.latitudeS = dg[1].Replace(',', '.');
                            }
                            else if (dataCount == 18)
                            {
                                String[] dg = datalar.Split(' ');
                                koordinat.longitudeS = dg[1].Replace(',', '.');
                            }
                            if (dataCount < _firstLine)
                            {
                                continue;
                            }
                            else
                            {
                                datalar = datalar.Replace('.', ',');
                                double aData = Convert.ToDouble(datalar);
                                double vData = 0.0, dData = 0.0;
                                if (dataCount == _firstLine)
                                {
                                    preAcceleration = aData;
                                    preVelocity = vData;
                                    preDisplacement = dData;
                                }
                                else
                                {
                                    vData = preVelocity + (((aData + preAcceleration) / 2) * _timeStep);
                                    dData = preDisplacement + (((vData + preVelocity) / 2) * _timeStep);
                                    preAcceleration = aData;
                                    preVelocity = vData;
                                    preDisplacement = dData;
                                }
                                if (i == 0)
                                {
                                    dataPoints1.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints6.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints11.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 1)
                                {
                                    dataPoints2.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints7.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints12.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 2)
                                {
                                    dataPoints3.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints8.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints13.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 3)
                                {
                                    dataPoints4.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints9.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints14.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 4)
                                {
                                    dataPoints5.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints10.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints15.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                timeScale = timeScale + _timeStep;
                                if (aData >= 0)
                                {
                                    if (aData > maxp)
                                    {
                                        maxp = aData;
                                    }
                                }
                                else
                                {
                                    if (aData * (-1) > maxn * (-1))
                                    {
                                        maxn = aData;
                                    }
                                }
                            }
                        }
                        lines.Close();
                    }
                    catch (Exception e)
                    {
                        lines.Close();
                        for (int j = 0; j < 5; j++)
                        {
                            Koordinat boskoordinat = new Koordinat();
                            boskoordinat.dosyaAdi = "";
                            boskoordinat.latitudeE = "0.0000";
                            boskoordinat.longitudeE = "0.0000";
                            boskoordinat.latitudeS = "0.0000";
                            boskoordinat.longitudeS = "0.0000";
                            koordinatlar.Add(boskoordinat);
                        }
                        ViewBag.Message = "The files could not be analyzed!";
                        return View(koordinatlar);
                    }
                }
                else if (_fileType.Equals("Time"))
                {
                    //
                }
                else if (_fileType.Equals("Multiple"))
                {
                    //
                }
                else if (_fileType.Equals("Peer"))
                {
                    StreamReader lines = null;
                    try
                    {
                        var file = Request.Files[i];
                        koordinat.dosyaAdi = file.FileName;
                        lines = System.IO.File.OpenText(dosyaYolu[i]);
                        String datalar = null;
                        int dataCount = 0;
                        while ((datalar = lines.ReadLine()) != null)
                        {
                            dataCount++;

                            koordinat.latitudeE = form["Latitude"];
                            koordinat.longitudeE = form["Longitude"];
                            koordinat.latitudeS = form["Latitude1"];
                            koordinat.longitudeS = form["Longitude1"];

                            if (dataCount < _firstLine)
                            {
                                continue;
                            }
                            else
                            {
                                double aData = 0.0;
                                String[] _data = datalar.Split(' ');
                                foreach (string _veri in _data)
                                {
                                    if (_veri != "")
                                    {
                                        String value = _veri;
                                        value = value.Replace('.', ',');
                                        aData = Convert.ToDouble(value);
                                    }
                                }
                                double vData = 0.0, dData = 0.0;
                                if (dataCount == _firstLine)
                                {
                                    preAcceleration = aData;
                                    preVelocity = vData;
                                    preDisplacement = dData;
                                }
                                else
                                {
                                    vData = preVelocity + (((aData + preAcceleration) / 2) * _timeStep);
                                    dData = preDisplacement + (((vData + preVelocity) / 2) * _timeStep);
                                    preAcceleration = aData;
                                    preVelocity = vData;
                                    preDisplacement = dData;
                                }
                                if (i == 0)
                                {
                                    dataPoints1.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints6.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints11.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 1)
                                {
                                    dataPoints2.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints7.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints12.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 2)
                                {
                                    dataPoints3.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints8.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints13.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 3)
                                {
                                    dataPoints4.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints9.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints14.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                else if (i == 4)
                                {
                                    dataPoints5.Add(new DataPoint(timeScale, aData, 0, 0));
                                    dataPoints10.Add(new DataPoint(timeScale, vData, 0, 0));
                                    dataPoints15.Add(new DataPoint(timeScale, dData, 0, 0));
                                }
                                timeScale = timeScale + _timeStep;
                                if (aData >= 0)
                                {
                                    if (aData > maxp)
                                    {
                                        maxp = aData;
                                    }
                                }
                                else
                                {
                                    if (aData * (-1) > maxn * (-1))
                                    {
                                        maxn = aData;
                                    }
                                }
                            }
                        }
                        lines.Close();
                    }
                    catch (Exception e)
                    {
                        lines.Close();
                        for (int j = 0; j < 5; j++)
                        {
                            Koordinat boskoordinat = new Koordinat();
                            boskoordinat.dosyaAdi = "";
                            boskoordinat.latitudeE = "0.0000";
                            boskoordinat.longitudeE = "0.0000";
                            boskoordinat.latitudeS = "0.0000";
                            boskoordinat.longitudeS = "0.0000";
                            koordinatlar.Add(boskoordinat);
                        }
                        ViewBag.Message = "The files could not be analyzed!";
                        return View(koordinatlar);
                    }

                }
                //Peer Son
                double i_e = 41.619888;
                double i_b = 29.973888;
                double X1 = Convert.ToDouble(koordinat.latitudeE.Replace('.', ','));
                double Y1 = Convert.ToDouble(koordinat.longitudeE.Replace('.', ','));
                double X2= Convert.ToDouble(koordinat.latitudeS.Replace('.', ','));
                double Y2 = Convert.ToDouble(koordinat.longitudeS.Replace('.', ','));

               
                double Enlem1 = X1 / 180 * Math.PI;
                double Boylam1 = Y1 / 180 * Math.PI;
                double Enlem2 = X2 / 180 * Math.PI;
                double Boylam2 = Y2 / 180 * Math.PI;
                double Enlem3 = i_e / 180 * Math.PI;
                double Boylam3 = i_b / 180 * Math.PI;
                
                double mesafe = Math.Acos(Math.Sin(Boylam1) * Math.Sin(Boylam2) + Math.Cos(Boylam1) * Math.Cos(Boylam2) * Math.Cos(Enlem2 - Enlem1)) * 6731;
                koordinat.Distance1 = mesafe.ToString("N4").Replace(".", "").Replace(",", ".") + " km";
                double mesafe2 = Math.Acos(Math.Sin(Boylam1) * Math.Sin(Boylam3) + Math.Cos(Boylam1) * Math.Cos(Boylam3) * Math.Cos(Enlem3 - Enlem1)) * 6731;
                koordinat.Distance2 = mesafe2.ToString("N4").Replace("." , "").Replace(",", ".") + " km";

                if (i == 0)
                {
                    foreach (var item in dataPoints1)
                    {
                        item.MAXP = maxp;
                        item.MAXN = maxn;
                    }
                }
                else if (i == 1)
                {
                    foreach (var item in dataPoints2)
                    {
                        item.MAXP = maxp;
                        item.MAXN = maxn;
                    }
                }
                else if (i == 2)
                {
                    foreach (var item in dataPoints3)
                    {
                        item.MAXP = maxp;
                        item.MAXN = maxn;
                    }
                }
                else if (i == 3)
                {
                    foreach (var item in dataPoints4)
                    {
                        item.MAXP = maxp;
                        item.MAXN = maxn;
                    }
                }
                else if (i == 4)
                {
                    foreach (var item in dataPoints5)
                    {
                        item.MAXP = maxp;
                        item.MAXN = maxn;
                    }
                }
                koordinatlar.Add(koordinat);
            }
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);
            ViewBag.DataPoints4 = JsonConvert.SerializeObject(dataPoints4);
            ViewBag.DataPoints5 = JsonConvert.SerializeObject(dataPoints5);
            ViewBag.DataPoints6 = JsonConvert.SerializeObject(dataPoints6);
            ViewBag.DataPoints7 = JsonConvert.SerializeObject(dataPoints7);
            ViewBag.DataPoints8 = JsonConvert.SerializeObject(dataPoints8);
            ViewBag.DataPoints9 = JsonConvert.SerializeObject(dataPoints9);
            ViewBag.DataPoints10 = JsonConvert.SerializeObject(dataPoints10);
            ViewBag.DataPoints11 = JsonConvert.SerializeObject(dataPoints11);
            ViewBag.DataPoints12 = JsonConvert.SerializeObject(dataPoints12);
            ViewBag.DataPoints13 = JsonConvert.SerializeObject(dataPoints13);
            ViewBag.DataPoints14 = JsonConvert.SerializeObject(dataPoints14);
            ViewBag.DataPoints15 = JsonConvert.SerializeObject(dataPoints15);

            


            if (koordinatlar.Count != 5)
            {
                for (int i = koordinatlar.Count; i < 5; i++)
                {
                    Koordinat boskoordinat = new Koordinat();
                    boskoordinat.dosyaAdi = "";
                    boskoordinat.latitudeE = "0.0000";
                    boskoordinat.longitudeE = "0.0000";
                    boskoordinat.latitudeS = "0.0000";
                    boskoordinat.longitudeS = "0.0000";
                    koordinatlar.Add(boskoordinat);
                }
            }
           

            return View(koordinatlar);
            

        }


    }
}