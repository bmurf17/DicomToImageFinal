using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.NativeCodec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.IO.Compression;

namespace DicomToImageFinal.Controllers
{
    [Route("api/clinics")]
    [ApiController]
    public class DICOMConvert : ControllerBase
    {
        [HttpGet("BitmapAttempt")]
        public async Task<ActionResult> BitmapAttempt()
        {
            new DicomSetupBuilder()
                .RegisterServices(s => s
                    .AddFellowOakDicom()
                    .AddTranscoderManager<NativeTranscoderManager>()
                    .AddImageManager<ImageSharpImageManager>())
                .Build();

            //change with a path to your dicom image
            var dicomImage = new DicomImage("C:\\DICOM\\1.2.840.114257.1.999.1.1.9.20221216173119.dcm");
            using var img = dicomImage.RenderImage();
            using var sharpImage = img.AsSharpImage();

            using var ms = new MemoryStream();
            sharpImage.Save(ms, new SixLabors.ImageSharp.Formats.Png.PngEncoder());

            //write wherever you want to save the png
            System.IO.File.WriteAllBytes("C:\\DICOM\\thisIsATest4.png", ms.ToArray());

            return Ok();
        }
    }
}