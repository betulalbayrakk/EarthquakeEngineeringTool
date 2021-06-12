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
            double maxpA1 = 0, maxpA2 = 0, maxpA3 = 0, maxpA4 = 0, maxpA5 = 0, maxnA1 = 0, maxnA2 = 0, maxnA3 = 0, maxnA4 = 0, maxnA5 = 0;
            double maxpV1 = 0, maxpV2 = 0, maxpV3 = 0, maxpV4 = 0, maxpV5 = 0, maxnV1 = 0, maxnV2 = 0, maxnV3 = 0, maxnV4 = 0, maxnV5 = 0;
            double maxpD1 = 0, maxpD2 = 0, maxpD3 = 0, maxpD4 = 0, maxpD5 = 0, maxnD1 = 0, maxnD2 = 0, maxnD3 = 0, maxnD4 = 0, maxnD5 = 0;
            double squareA1=0, squareA2=0, squareA3=0,  squareA4=0, squareA5 = 0;
            double squareV1=0, squareV2=0, squareV3=0,  squareV4=0, squareV5 = 0;
            double squareD1=0, squareD2=0, squareD3=0,  squareD4=0, squareD5 = 0;
            double rmsA1=0, rmsA2=0, rmsA3=0, rmsA4=0, rmsA5 = 0;
            double rmsV1=0, rmsV2=0, rmsV3=0, rmsV4=0, rmsV5 = 0;
            double rmsD1=0, rmsD2=0, rmsD3=0, rmsD4=0, rmsD5 = 0;
            double ratio1=0, ratio2=0, ratio3=0, ratio4=0, ratio5 = 0;
            double arInt1=0, arInt2=0, arInt3=0, arInt4=0, arInt5 = 0;
            double chaInt1=0, chaInt2=0, chaInt3=0, chaInt4=0, chaInt5 = 0;
            double tD = 1;
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

            int maxIndexA1 = -1, maxIndexA2 = -1, maxIndexA3 = -1, maxIndexA4 = -1, maxIndexA5 = -1;
            int maxIndexV1 = -1, maxIndexV2 = -1, maxIndexV3 = -1, maxIndexV4 = -1, maxIndexV5 = -1;
            int maxIndexD1 = -1, maxIndexD2 = -1, maxIndexD3 = -1, maxIndexD4 = -1, maxIndexD5 = -1;
            string maxValueA1 = null, maxValueA2 = null, maxValueA3 = null, maxValueA4 = null, maxValueA5 = null;
            string maxValueV1 = null, maxValueV2 = null, maxValueV3 = null, maxValueV4 = null, maxValueV5 = null;
            string maxValueD1 = null, maxValueD2 = null, maxValueD3 = null, maxValueD4 = null, maxValueD5 = null;

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
                                double aData = (Convert.ToDouble(datalar)) / 100;     // cm/s^2 = 1/100 m/s^2
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
                                    dataPoints1.Add(new DataPoint(timeScale, aData));
                                    dataPoints6.Add(new DataPoint(timeScale, vData));
                                    dataPoints11.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA1)
                                        {
                                            maxpA1 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA1 * (-1))
                                        {
                                            maxnA1 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV1)
                                        {
                                            maxpV1 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV1 * (-1))
                                        {
                                            maxnV1 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD1)
                                        {
                                            maxpD1 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD1 * (-1))
                                        {
                                            maxnD1 = dData;
                                        }
                                    }
                                }
                                else if (i == 1)
                                {
                                    dataPoints2.Add(new DataPoint(timeScale, aData));
                                    dataPoints7.Add(new DataPoint(timeScale, vData));
                                    dataPoints12.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA2)
                                        {
                                            maxpA2 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA2 * (-1))
                                        {
                                            maxnA2 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV2)
                                        {
                                            maxpV2 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV2 * (-1))
                                        {
                                            maxnV2 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD2)
                                        {
                                            maxpD2 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD2 * (-1))
                                        {
                                            maxnD2 = dData;
                                        }
                                    }
                                }
                                else if (i == 2)
                                {
                                    dataPoints3.Add(new DataPoint(timeScale, aData));
                                    dataPoints8.Add(new DataPoint(timeScale, vData));
                                    dataPoints13.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA3)
                                        {
                                            maxpA3 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA3 * (-1))
                                        {
                                            maxnA3 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV3)
                                        {
                                            maxpV3 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV3 * (-1))
                                        {
                                            maxnV3 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD3)
                                        {
                                            maxpD3 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD3 * (-1))
                                        {
                                            maxnD3 = dData;
                                        }
                                    }
                                }
                                else if (i == 3)
                                {
                                    dataPoints4.Add(new DataPoint(timeScale, aData));
                                    dataPoints9.Add(new DataPoint(timeScale, vData));
                                    dataPoints14.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA4)
                                        {
                                            maxpA4 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA4 * (-1))
                                        {
                                            maxnA4 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV4)
                                        {
                                            maxpV4 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV4 * (-1))
                                        {
                                            maxnV4 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD4)
                                        {
                                            maxpD4 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD4 * (-1))
                                        {
                                            maxnD4 = dData;
                                        }
                                    }
                                }
                                else if (i == 4)
                                {
                                    dataPoints5.Add(new DataPoint(timeScale, aData));
                                    dataPoints10.Add(new DataPoint(timeScale, vData));
                                    dataPoints15.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA5)
                                        {
                                            maxpA5 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA5 * (-1))
                                        {
                                            maxnA5 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV5)
                                        {
                                            maxpV5 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV5 * (-1))
                                        {
                                            maxnV5 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD5)
                                        {
                                            maxpD5 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD5 * (-1))
                                        {
                                            maxnD5 = dData;
                                        }
                                    }
                                }
                                timeScale = timeScale + _timeStep;
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
                                        aData = (Convert.ToDouble(value)) * 9.80665;
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
                                    dataPoints1.Add(new DataPoint(timeScale, aData));
                                    dataPoints6.Add(new DataPoint(timeScale, vData));
                                    dataPoints11.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA1)
                                        {
                                            maxpA1 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA1 * (-1))
                                        {
                                            maxnA1 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV1)
                                        {
                                            maxpV1 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV1 * (-1))
                                        {
                                            maxnV1 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD1)
                                        {
                                            maxpD1 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD1 * (-1))
                                        {
                                            maxnD1 = dData;
                                        }
                                    }
                                }
                                else if (i == 1)
                                {
                                    dataPoints2.Add(new DataPoint(timeScale, aData));
                                    dataPoints7.Add(new DataPoint(timeScale, vData));
                                    dataPoints12.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA2)
                                        {
                                            maxpA2 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA2 * (-1))
                                        {
                                            maxnA2 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV2)
                                        {
                                            maxpV2 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV2 * (-1))
                                        {
                                            maxnV2 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD2)
                                        {
                                            maxpD2 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD2 * (-1))
                                        {
                                            maxnD2 = dData;
                                        }
                                    }
                                }
                                else if (i == 2)
                                {
                                    dataPoints3.Add(new DataPoint(timeScale, aData));
                                    dataPoints8.Add(new DataPoint(timeScale, vData));
                                    dataPoints13.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA3)
                                        {
                                            maxpA3 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA3 * (-1))
                                        {
                                            maxnA3 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV3)
                                        {
                                            maxpV3 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV3 * (-1))
                                        {
                                            maxnV3 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD3)
                                        {
                                            maxpD3 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD3 * (-1))
                                        {
                                            maxnD3 = dData;
                                        }
                                    }
                                }
                                else if (i == 3)
                                {
                                    dataPoints4.Add(new DataPoint(timeScale, aData));
                                    dataPoints9.Add(new DataPoint(timeScale, vData));
                                    dataPoints14.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA4)
                                        {
                                            maxpA4 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA4 * (-1))
                                        {
                                            maxnA4 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV4)
                                        {
                                            maxpV4 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV4 * (-1))
                                        {
                                            maxnV4 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD4)
                                        {
                                            maxpD4 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD4 * (-1))
                                        {
                                            maxnD4 = dData;
                                        }
                                    }
                                }
                                else if (i == 4)
                                {
                                    dataPoints5.Add(new DataPoint(timeScale, aData));
                                    dataPoints10.Add(new DataPoint(timeScale, vData));
                                    dataPoints15.Add(new DataPoint(timeScale, dData));
                                    if (aData >= 0)
                                    {
                                        if (aData > maxpA5)
                                        {
                                            maxpA5 = aData;
                                        }
                                    }
                                    else
                                    {
                                        if (aData * (-1) > maxnA5 * (-1))
                                        {
                                            maxnA5 = aData;
                                        }
                                    }
                                    if (vData >= 0)
                                    {
                                        if (vData > maxpV5)
                                        {
                                            maxpV5 = vData;
                                        }
                                    }
                                    else
                                    {
                                        if (vData * (-1) > maxnV5 * (-1))
                                        {
                                            maxnV5 = vData;
                                        }
                                    }
                                    if (dData >= 0)
                                    {
                                        if (dData > maxpD5)
                                        {
                                            maxpD5 = dData;
                                        }
                                    }
                                    else
                                    {
                                        if (dData * (-1) > maxnD5 * (-1))
                                        {
                                            maxnD5 = dData;
                                        }
                                    }
                                }
                                timeScale = timeScale + _timeStep;
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
                    for (int k = 0; k < dataPoints1.Count; k++) // A1
                    {
                        squareA1 += Math.Pow(Convert.ToDouble(dataPoints1[k].Y), 2);
                        dataPoints1[k].Y /= 9.80665;
                    }
                    maxpA1 /= 9.80665;
                    maxnA1 /= 9.80665;
                    rmsA1 = squareA1 / dataPoints1.Count;
                    rmsA1 = Math.Sqrt(rmsA1);
                    for (int k = 0; k < dataPoints6.Count; k++)
                    {
                        squareV1 += Math.Pow(Convert.ToDouble(dataPoints6[k].Y), 2);
                    }
                    for (int k = 0; k < dataPoints11.Count; k++)
                    {
                        squareD1 += Math.Pow(Convert.ToDouble(dataPoints11[k].Y), 2);
                    }
                    rmsV1 = squareV1 / dataPoints6.Count;
                    rmsV1 = Math.Sqrt(rmsV1);
                    rmsD1 = squareD1 / dataPoints11.Count;
                    rmsD1 = Math.Sqrt(rmsD1);
                    arInt1 = (3.14159 / (2 * 9.80665)) * squareA1;
                    chaInt1 = Math.Pow(rmsA1, 1.5) * Math.Pow(tD, 0.5);  
                    
                    double maxA1 = 0, maxV1 = 0;

                    if (maxpA1 >= (maxnA1 * (-1)))
                    {
                        for (int k = 0; k < dataPoints1.Count; k++)
                        {
                            if (dataPoints1[k].Y == maxpA1)
                            {
                                maxIndexA1 = k;
                                maxValueA1 = maxpA1.ToString("N2") + " G";
                                maxA1 = maxpA1;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints1.Count; k++)
                        {
                            if (dataPoints1[k].Y == maxnA1)
                            {
                                maxIndexA1 = k;
                                maxValueA1 = (maxnA1 * (-1)).ToString("N2") + " G";
                                maxA1 = maxnA1 * (-1);
                            }
                        }
                    }
                    if (maxpV1 >= (maxnV1 * (-1)))
                    {
                        for (int k = 0; k < dataPoints6.Count; k++)
                        {
                            if (dataPoints6[k].Y == maxpV1)
                            {
                                maxIndexV1 = k;
                                maxValueV1 = maxpV1.ToString("N1") + " m/s";
                                maxV1 = maxpV1;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints6.Count; k++)
                        {
                            if (dataPoints6[k].Y == maxnV1)
                            {
                                maxIndexV1 = k;
                                maxValueV1 = (maxnV1 * (-1)).ToString("N1") + " m/s";
                                maxV1 = maxnV1 * (-1);
                            }
                        }
                    }

                    ratio1 = maxV1 / maxA1;

                    if (maxpD1 >= (maxnD1 * (-1)))
                    {
                        for (int k = 0; k < dataPoints11.Count; k++)
                        {
                            if (dataPoints11[k].Y == maxpD1)
                            {
                                maxIndexD1 = k;
                                maxValueD1 = maxpD1.ToString("N1") + " m";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints11.Count; k++)
                        {
                            if (dataPoints11[k].Y == maxnD1)
                            {
                                maxIndexD1 = k;
                                maxValueD1 = maxnD1.ToString("N1") + " m";
                            }
                        }
                    }
                }
                else if (i == 1)
                {
                    for (int k = 0; k < dataPoints2.Count; k++) // A2
                    {
                        dataPoints2[k].Y /= 9.80665;
                    }
                    maxpA2 /= 9.80665;
                    maxnA2 /= 9.80665;
                    rmsA2 = squareA2 / dataPoints2.Count;
                    rmsA2 = Math.Sqrt(rmsA2);
                    for (int k = 0; k < dataPoints7.Count; k++)
                    {
                        squareV2 += Math.Pow(Convert.ToDouble(dataPoints7[k].Y), 2);
                    }
                    for (int k = 0; k < dataPoints12.Count; k++)
                    {
                        squareD2 += Math.Pow(Convert.ToDouble(dataPoints12[k].Y), 2);
                    }
                    rmsV2 = squareV2 / dataPoints7.Count;
                    rmsV2 = Math.Sqrt(rmsV2);
                    rmsD2 = squareD2 / dataPoints12.Count;
                    rmsD2 = Math.Sqrt(rmsD2);
                    arInt2 = (3.14159 / (2 * 9.80665)) * squareA2;
                    chaInt2 = Math.Pow(rmsA2, 1.5) * Math.Pow(tD, 0.5);


                    double maxA2 = 0, maxV2 = 0;
                    if (maxpA2 >= (maxnA2 * (-1)))
                    {
                        for (int k = 0; k < dataPoints2.Count; k++)
                        {
                            if (dataPoints2[k].Y == maxpA2)
                            {
                                maxIndexA2 = k;
                                maxValueA2 = maxpA2.ToString("N2") + " G";
                                maxA2 = maxpA2;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints2.Count; k++)
                        {
                            if (dataPoints2[k].Y == maxnA2)
                            {
                                maxIndexA2 = k;
                                maxValueA2 = maxnA2.ToString("N2") + " G";
                                maxA2 = maxnA2;
                            }
                        }
                    }
                    if (maxpV2 >= (maxnV2 * (-1)))
                    {
                        for (int k = 0; k < dataPoints7.Count; k++)
                        {
                            if (dataPoints7[k].Y == maxpV2)
                            {
                                maxIndexV2 = k;
                                maxValueV2 = maxpV2.ToString("N1") + " m/s";
                                maxV2 = maxpV2;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints7.Count; k++)
                        {
                            if (dataPoints7[k].Y == maxnV2)
                            {
                                maxIndexV2 = k;
                                maxValueV2 = maxnV2.ToString("N1") + " m/s";
                                maxV2 = maxnV2;
                            }
                        }
                    }

                    ratio2 = maxV2 / maxA2;
                    if (maxpD2 >= (maxnD2 * (-1)))
                    {
                        for (int k = 0; k < dataPoints12.Count; k++)
                        {
                            if (dataPoints12[k].Y == maxpD2)
                            {
                                maxIndexD2 = k;
                                maxValueD2 = maxpD2.ToString("N1") + " m";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints12.Count; k++)
                        {
                            if (dataPoints12[k].Y == maxnD2)
                            {
                                maxIndexD2 = k;
                                maxValueD2 = maxnD2.ToString("N1") + " m";
                            }
                        }
                    }
                }
                else if (i == 2)
                {
                    for (int k = 0; k < dataPoints3.Count; k++) // A3
                    {
                        dataPoints3[k].Y /= 9.80665;
                    }
                    maxpA3 /= 9.80665;
                    maxnA3 /= 9.80665;
                    rmsA3 = squareA3 / dataPoints3.Count;
                    rmsA3 = Math.Sqrt(rmsA3);
                    for (int k = 0; k < dataPoints8.Count; k++)
                    {
                        squareV3 += Math.Pow(Convert.ToDouble(dataPoints8[k].Y), 2);
                    }
                    for (int k = 0; k < dataPoints13.Count; k++)
                    {
                        squareD3 += Math.Pow(Convert.ToDouble(dataPoints13[k].Y), 2);
                    }
                    rmsV3 = squareV3 / dataPoints8.Count;
                    rmsV3 = Math.Sqrt(rmsV3);
                    rmsD3 = squareD3 / dataPoints13.Count;
                    rmsD3 = Math.Sqrt(rmsD3);
                    arInt3 = (3.14159 / (2 * 9.80665)) * squareA3;
                    chaInt3 = Math.Pow(rmsA3, 1.5) * Math.Pow(tD, 0.5);


                    double maxA3 = 0, maxV3 = 0;
                    if (maxpA3 >= (maxnA3 * (-1)))
                    {
                        for (int k = 0; k < dataPoints3.Count; k++)
                        {
                            if (dataPoints3[k].Y == maxpA3)
                            {
                                maxIndexA3 = k;
                                maxValueA3 = maxpA3.ToString("N2") + " G";
                                maxA3 = maxpA3;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints3.Count; k++)
                        {
                            if (dataPoints3[k].Y == maxnA3)
                            {
                                maxIndexA3 = k;
                                maxValueA3 = maxnA3.ToString("N2") + " G";
                                maxA3 = maxnA3;
                            }
                        }
                    }
                    if (maxpV3 >= (maxnV3 * (-1)))
                    {
                        for (int k = 0; k < dataPoints8.Count; k++)
                        {
                            if (dataPoints8[k].Y == maxpV3)
                            {
                                maxIndexV3 = k;
                                maxValueV3 = maxpV3.ToString("N1") + " m/s";
                                maxV3 = maxpV3;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints8.Count; k++)
                        {
                            if (dataPoints8[k].Y == maxnV3)
                            {
                                maxIndexV3 = k;
                                maxValueV3 = maxnV3.ToString("N1") + " m/s";
                                maxV3 = maxnV3;
                            }
                        }
                    }
                    ratio3 = maxV3 / maxA3;
                    if (maxpD3 >= (maxnD3 * (-1)))
                    {
                        for (int k = 0; k < dataPoints13.Count; k++)
                        {
                            if (dataPoints13[k].Y == maxpD3)
                            {
                                maxIndexD3 = k;
                                maxValueD3 = maxpD3.ToString("N1") + " m";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints13.Count; k++)
                        {
                            if (dataPoints13[k].Y == maxnD3)
                            {
                                maxIndexD3 = k;
                                maxValueD3 = maxnD3.ToString("N1") + " m";
                            }
                        }
                    }
                }
                else if (i == 3)
                {
                    for (int k = 0; k < dataPoints4.Count; k++) // A4
                    {
                        dataPoints4[k].Y /= 9.80665;
                    }
                    maxpA4 /= 9.80665;
                    maxnA4 /= 9.80665;
                    rmsA4 = squareA4 / dataPoints4.Count;
                    rmsA4 = Math.Sqrt(rmsA4);
                    for (int k = 0; k < dataPoints9.Count; k++)
                    {
                        squareV4 += Math.Pow(Convert.ToDouble(dataPoints9[k].Y), 2);
                    }
                    for (int k = 0; k < dataPoints14.Count; k++)
                    {
                        squareD4 += Math.Pow(Convert.ToDouble(dataPoints14[k].Y), 2);
                    }
                    rmsV4 = squareV4 / dataPoints9.Count;
                    rmsV4 = Math.Sqrt(rmsV4);
                    rmsD4 = squareD4 / dataPoints14.Count;
                    rmsD3 = Math.Sqrt(rmsD3);
                    arInt4 = (3.14159 / (2 * 9.80665)) * squareA4;
                    chaInt4 = Math.Pow(rmsA4, 1.5) * Math.Pow(tD, 0.5);


                    double maxA4 = 0, maxV4 = 0;
                    if (maxpA4 >= (maxnA4 * (-1)))
                    {
                        for (int k = 0; k < dataPoints4.Count; k++)
                        {
                            if (dataPoints4[k].Y == maxpA4)
                            {
                                maxIndexA4 = k;
                                maxValueA4 = maxpA4.ToString("N2") + " G";
                                maxA4 = maxpA4;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints4.Count; k++)
                        {
                            if (dataPoints4[k].Y == maxnA4)
                            {
                                maxIndexA4 = k;
                                maxValueA4 = maxnA4.ToString("N2") + " G";
                                maxA4 = maxnA4;
                            }
                        }
                    }
                    if (maxpV4 >= (maxnV4 * (-1)))
                    {
                        for (int k = 0; k < dataPoints9.Count; k++)
                        {
                            if (dataPoints9[k].Y == maxpV4)
                            {
                                maxIndexV4 = k;
                                maxValueV4 = maxpV4.ToString("N1") + " m/s";
                                maxV4 = maxpV4;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints9.Count; k++)
                        {
                            if (dataPoints9[k].Y == maxnV4)
                            {
                                maxIndexV4 = k;
                                maxValueV4 = maxnV4.ToString("N1") + " m/s";
                                maxV4 = maxnV4;
                            }
                        }
                    }
                    ratio4 = maxV4 / maxA4;

                    if (maxpD4 >= (maxnD4 * (-1)))
                    {
                        for (int k = 0; k < dataPoints14.Count; k++)
                        {
                            if (dataPoints14[k].Y == maxpD4)
                            {
                                maxIndexD4 = k;
                                maxValueD4 = maxpD4.ToString("N1") + " m";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints14.Count; k++)
                        {
                            if (dataPoints14[k].Y == maxnD4)
                            {
                                maxIndexD4 = k;
                                maxValueD4 = maxnD4.ToString("N1") + " m";
                            }
                        }
                    }
                }
                else if (i == 4)
                {
                    for (int k = 0; k < dataPoints5.Count; k++) // A5
                    {
                        dataPoints5[k].Y /= 9.80665;
                    }
                    maxpA5 /= 9.80665;
                    maxnA5 /= 9.80665;
                    rmsA5 = squareA5 / dataPoints5.Count;
                    rmsA5 = Math.Sqrt(rmsA5);
                    for (int k = 0; k < dataPoints9.Count; k++)
                    {
                        squareV4 += Math.Pow(Convert.ToDouble(dataPoints9[k].Y), 2);
                    }
                    for (int k = 0; k < dataPoints14.Count; k++)
                    {
                        squareD4 += Math.Pow(Convert.ToDouble(dataPoints14[k].Y), 2);
                    }
                    rmsV4 = squareV4 / dataPoints9.Count;
                    rmsV4 = Math.Sqrt(rmsV4);
                    rmsD4 = squareD4 / dataPoints14.Count;
                    rmsD3 = Math.Sqrt(rmsD3);
                    arInt4 = (3.14159 / (2 * 9.80665)) * squareA4;
                    chaInt4 = Math.Pow(rmsA4, 1.5) * Math.Pow(tD, 0.5);


                    double maxA4 = 0, maxV4 = 0;
                    if (maxpA5 >= (maxnA5 * (-1)))
                    {
                        for (int k = 0; k < dataPoints5.Count; k++)
                        {
                            if (dataPoints5[k].Y == maxpA5)
                            {
                                maxIndexA5 = k;
                                maxValueA5 = maxpA5.ToString("N2") + " G";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints5.Count; k++)
                        {
                            if (dataPoints5[k].Y == maxnA5)
                            {
                                maxIndexA5 = k;
                                maxValueA5 = maxnA5.ToString("N2") + " G";
                            }
                        }
                    }
                    if (maxpV5 >= (maxnV5 * (-1)))
                    {
                        for (int k = 0; k < dataPoints10.Count; k++)
                        {
                            if (dataPoints10[k].Y == maxpV5)
                            {
                                maxIndexV5 = k;
                                maxValueV5 = maxpV5.ToString("N1") + " m/s";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints10.Count; k++)
                        {
                            if (dataPoints10[k].Y == maxnV5)
                            {
                                maxIndexV5 = k;
                                maxValueV5 = maxnV5.ToString("N1") + " m/s";
                            }
                        }
                    }
                    if (maxpD5 >= (maxnD5 * (-1)))
                    {
                        for (int k = 0; k < dataPoints15.Count; k++)
                        {
                            if (dataPoints15[k].Y == maxpD5)
                            {
                                maxIndexD5 = k;
                                maxValueD5 = maxpD5.ToString("N1") + " m";
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < dataPoints15.Count; k++)
                        {
                            if (dataPoints15[k].Y == maxnD5)
                            {
                                maxIndexD5 = k;
                                maxValueD5 = maxnD5.ToString("N1") + " m";
                            }
                        }
                    }
                }

                koordinat.PGA1 = maxValueA1;
                koordinat.PGA2 = maxValueA2;
                koordinat.PGA3 = maxValueA3;
                koordinat.PGA4 = maxValueA4;
                koordinat.PGA5 = maxValueA5;

                koordinat.PGV1 = maxValueV1;
                koordinat.PGV2 = maxValueV2;
                koordinat.PGV3 = maxValueV3;
                koordinat.PGV4 = maxValueV4;
                koordinat.PGV5 = maxValueV5;

                koordinat.PGD1 = maxValueD1;
                koordinat.PGD2 = maxValueD2;
                koordinat.PGD3 = maxValueD3;
                koordinat.PGD4 = maxValueD4;
                koordinat.PGD5 = maxValueD5;

                koordinat.RMSA1 = rmsA1.ToString("N3");

                koordinat.RMSV1 = rmsV1.ToString("N3");

                koordinat.RMSD1 = rmsD1.ToString("N3");

                koordinat.ratio1 = ratio1.ToString("N3");

                koordinat.ArInt1 = arInt1.ToString("N3");

                koordinat.ChaInt1 = chaInt1.ToString("N3");

                koordinatlar.Add(koordinat);
            }
            string _dataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            string[] _parcalaData1 = _dataPoints1.Split('}');
            _dataPoints1 = null;
            for (int k = 0; k < _parcalaData1.Length; k++)
            {
                if (k != maxIndexA1 && k != _parcalaData1.Length - 1)
                {
                    _dataPoints1 += _parcalaData1[k] + "}";
                }
                else if (k == maxIndexA1 && k != _parcalaData1.Length - 1)
                {
                    _dataPoints1 += _parcalaData1[k] + ",\"indexLabel\":\"%PGA:" + maxValueA1 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints1 += _parcalaData1[k];
                }
            }
            string _dataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            string[] _parcalaData2 = _dataPoints2.Split('}');
            _dataPoints2 = null;
            for (int k = 0; k < _parcalaData2.Length; k++)
            {
                if (k != maxIndexA2 && k != _parcalaData2.Length - 1)
                {
                    _dataPoints2 += _parcalaData2[k] + "}";
                }
                else if (k == maxIndexA2 && k != _parcalaData2.Length - 1)
                {
                    _dataPoints2 += _parcalaData2[k] + ",\"indexLabel\":\"%PGA:" + maxValueA2 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints2 += _parcalaData2[k];
                }
            }
            string _dataPoints3 = JsonConvert.SerializeObject(dataPoints3);
            string[] _parcalaData3 = _dataPoints3.Split('}');
            _dataPoints3 = null;
            for (int k = 0; k < _parcalaData3.Length; k++)
            {
                if (k != maxIndexA3 && k != _parcalaData3.Length - 1)
                {
                    _dataPoints3 += _parcalaData3[k] + "}";
                }
                else if (k == maxIndexA3 && k != _parcalaData3.Length - 1)
                {
                    _dataPoints3 += _parcalaData3[k] + ",\"indexLabel\":\"%PGA:" + maxValueA3 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints3 += _parcalaData3[k];
                }
            }
            string _dataPoints4 = JsonConvert.SerializeObject(dataPoints4);
            string[] _parcalaData4 = _dataPoints4.Split('}');
            _dataPoints4 = null;
            for (int k = 0; k < _parcalaData4.Length; k++)
            {
                if (k != maxIndexA4 && k != _parcalaData4.Length - 1)
                {
                    _dataPoints4 += _parcalaData4[k] + "}";
                }
                else if (k == maxIndexA4 && k != _parcalaData4.Length - 1)
                {
                    _dataPoints4 += _parcalaData4[k] + ",\"indexLabel\":\"%PGA:" + maxValueA4 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints4 += _parcalaData4[k];
                }
            }
            string _dataPoints5 = JsonConvert.SerializeObject(dataPoints5);
            string[] _parcalaData5 = _dataPoints5.Split('}');
            _dataPoints5 = null;
            for (int k = 0; k < _parcalaData5.Length; k++)
            {
                if (k != maxIndexA5 && k != _parcalaData5.Length - 1)
                {
                    _dataPoints5 += _parcalaData5[k] + "}";
                }
                else if (k == maxIndexA5 && k != _parcalaData5.Length - 1)
                {
                    _dataPoints5 += _parcalaData5[k] + ",\"indexLabel\":\"%PGA:" + maxValueA5 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints5 += _parcalaData5[k];
                }
            }
            string _dataPoints6 = JsonConvert.SerializeObject(dataPoints6);
            string[] _parcalaData6 = _dataPoints6.Split('}');
            _dataPoints6 = null;
            for (int k = 0; k < _parcalaData6.Length; k++)
            {
                if (k != maxIndexV1 && k != _parcalaData6.Length - 1)
                {
                    _dataPoints6 += _parcalaData6[k] + "}";
                }
                else if (k == maxIndexV1 && k != _parcalaData6.Length - 1)
                {
                    _dataPoints6 += _parcalaData6[k] + ",\"indexLabel\":\"%PGV:" + maxValueV1 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints6 += _parcalaData6[k];
                }
            }
            string _dataPoints7 = JsonConvert.SerializeObject(dataPoints7);
            string[] _parcalaData7 = _dataPoints7.Split('}');
            _dataPoints7 = null;
            for (int k = 0; k < _parcalaData7.Length; k++)
            {
                if (k != maxIndexV2 && k != _parcalaData7.Length - 1)
                {
                    _dataPoints7 += _parcalaData7[k] + "}";
                }
                else if (k == maxIndexV2 && k != _parcalaData7.Length - 1)
                {
                    _dataPoints7 += _parcalaData7[k] + ",\"indexLabel\":\"%PGV:" + maxValueV2 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints7 += _parcalaData7[k];
                }
            }
            string _dataPoints8 = JsonConvert.SerializeObject(dataPoints8);
            string[] _parcalaData8 = _dataPoints8.Split('}');
            _dataPoints8 = null;
            for (int k = 0; k < _parcalaData8.Length; k++)
            {
                if (k != maxIndexV3 && k != _parcalaData8.Length - 1)
                {
                    _dataPoints8 += _parcalaData8[k] + "}";
                }
                else if (k == maxIndexV3 && k != _parcalaData8.Length - 1)
                {
                    _dataPoints8 += _parcalaData8[k] + ",\"indexLabel\":\"%PGV:" + maxValueV3 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints8 += _parcalaData8[k];
                }
            }
            string _dataPoints9 = JsonConvert.SerializeObject(dataPoints9);
            string[] _parcalaData9 = _dataPoints9.Split('}');
            _dataPoints9 = null;
            for (int k = 0; k < _parcalaData9.Length; k++)
            {
                if (k != maxIndexV4 && k != _parcalaData9.Length - 1)
                {
                    _dataPoints9 += _parcalaData9[k] + "}";
                }
                else if (k == maxIndexV4 && k != _parcalaData9.Length - 1)
                {
                    _dataPoints9 += _parcalaData9[k] + ",\"indexLabel\":\"%PGV:" + maxValueV4 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints9 += _parcalaData9[k];
                }
            }
            string _dataPoints10 = JsonConvert.SerializeObject(dataPoints10);
            string[] _parcalaData10 = _dataPoints10.Split('}');
            _dataPoints10 = null;
            for (int k = 0; k < _parcalaData10.Length; k++)
            {
                if (k != maxIndexV5 && k != _parcalaData10.Length - 1)
                {
                    _dataPoints10 += _parcalaData10[k] + "}";
                }
                else if (k == maxIndexV5 && k != _parcalaData10.Length - 1)
                {
                    _dataPoints10 += _parcalaData10[k] + ",\"indexLabel\":\"%PGV:" + maxValueV5 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints10 += _parcalaData10[k];
                }
            }
            string _dataPoints11 = JsonConvert.SerializeObject(dataPoints11);
            string[] _parcalaData11 = _dataPoints11.Split('}');
            _dataPoints11 = null;
            for (int k = 0; k < _parcalaData11.Length; k++)
            {
                if (k != maxIndexD1 && k != _parcalaData11.Length - 1)
                {
                    _dataPoints11 += _parcalaData11[k] + "}";
                }
                else if (k == maxIndexD1 && k != _parcalaData11.Length - 1)
                {
                    _dataPoints11 += _parcalaData11[k] + ",\"indexLabel\":\"%PGD:" + maxValueD1 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints11 += _parcalaData11[k];
                }
            }
            string _dataPoints12 = JsonConvert.SerializeObject(dataPoints12);
            string[] _parcalaData12 = _dataPoints12.Split('}');
            _dataPoints12 = null;
            for (int k = 0; k < _parcalaData12.Length; k++)
            {
                if (k != maxIndexD2 && k != _parcalaData12.Length - 1)
                {
                    _dataPoints12 += _parcalaData12[k] + "}";
                }
                else if (k == maxIndexD2 && k != _parcalaData12.Length - 1)
                {
                    _dataPoints12 += _parcalaData12[k] + ",\"indexLabel\":\"%PGD:" + maxValueD2 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints12 += _parcalaData12[k];
                }
            }
            string _dataPoints13 = JsonConvert.SerializeObject(dataPoints13);
            string[] _parcalaData13 = _dataPoints13.Split('}');
            _dataPoints13 = null;
            for (int k = 0; k < _parcalaData13.Length; k++)
            {
                if (k != maxIndexD3 && k != _parcalaData13.Length - 1)
                {
                    _dataPoints13 += _parcalaData13[k] + "}";
                }
                else if (k == maxIndexD3 && k != _parcalaData13.Length - 1)
                {
                    _dataPoints13 += _parcalaData13[k] + ",\"indexLabel\":\"%PGD:" + maxValueD3 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints13 += _parcalaData13[k];
                }
            }
            string _dataPoints14 = JsonConvert.SerializeObject(dataPoints14);
            string[] _parcalaData14 = _dataPoints14.Split('}');
            _dataPoints14 = null;
            for (int k = 0; k < _parcalaData14.Length; k++)
            {
                if (k != maxIndexD4 && k != _parcalaData14.Length - 1)
                {
                    _dataPoints14 += _parcalaData14[k] + "}";
                }
                else if (k == maxIndexD4 && k != _parcalaData14.Length - 1)
                {
                    _dataPoints14 += _parcalaData14[k] + ",\"indexLabel\":\"%PGD:" + maxValueD4 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints14 += _parcalaData14[k];
                }
            }
            string _dataPoints15 = JsonConvert.SerializeObject(dataPoints15);
            string[] _parcalaData15 = _dataPoints15.Split('}');
            _dataPoints15 = null;
            for (int k = 0; k < _parcalaData15.Length; k++)
            {
                if (k != maxIndexD5 && k != _parcalaData15.Length - 1)
                {
                    _dataPoints15 += _parcalaData15[k] + "}";
                }
                else if (k == maxIndexD5 && k != _parcalaData15.Length - 1)
                {
                    _dataPoints15 += _parcalaData15[k] + ",\"indexLabel\":\"%PGD:" + maxValueD5 + "\",\"markerType\":\"triangle\",\"markerColor\":\"red\"}";
                }
                else
                {
                    _dataPoints15 += _parcalaData15[k];
                }
            }

            ViewBag.DataPoints1 = _dataPoints1;
            ViewBag.DataPoints2 = _dataPoints2;
            ViewBag.DataPoints3 = _dataPoints3;
            ViewBag.DataPoints4 = _dataPoints4;
            ViewBag.DataPoints5 = _dataPoints5;
            ViewBag.DataPoints6 = _dataPoints6;
            ViewBag.DataPoints7 = _dataPoints7;
            ViewBag.DataPoints8 = _dataPoints8;
            ViewBag.DataPoints9 = _dataPoints9;
            ViewBag.DataPoints10 = _dataPoints10;
            ViewBag.DataPoints11 = _dataPoints11;
            ViewBag.DataPoints12 = _dataPoints12;
            ViewBag.DataPoints13 = _dataPoints13;
            ViewBag.DataPoints14 = _dataPoints14;
            ViewBag.DataPoints15 = _dataPoints15;

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