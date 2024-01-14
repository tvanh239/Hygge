//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : AvatarService.cs　　                                        *
//* Function     : Create Avatar                                              *
//* Create       : VietAnh 2023/12/28                                         *
//*****************************************************************************.

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
namespace Hygge.Service
{

    // </summary>Create avatar</summary>
    public class AvatarService
    {

        public string? _url_avatar { get; set; }
        // </summary>Environment To Find Host</summary>
        private List<string> _BackgroundColours;
        // </summary>Environment To Find Host</summary>
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AvatarService(IWebHostEnvironment webHostEnvironment) {
            _url_avatar = null;
            _BackgroundColours = new List<string> { "#DB35A9", "#DB4D35", "#DB3445", "74ADB2", "#C035DB", "#9735DB", "#DB9ABE", "#6135DB" };
            _webHostEnvironment = webHostEnvironment;
        }
        public string? GeneratorAvatar(string userName)
        {
            try
            {
                //The first name char
                var avatarString = string.Format("{0}", userName[0]).ToUpper();

                //The choose Color
                var randomIndex = new Random().Next(_BackgroundColours.Count);
                var selectedColor = Color.ParseHex(_BackgroundColours[randomIndex]);

                // Get the physical path to wwwroot
                var wwwRootPath = _webHostEnvironment.WebRootPath;

                // Construct the file path within wwwroot/img
                var filePath = System.IO.Path.Combine(wwwRootPath, "img\\avatar", $"{avatarString}.png");

                // Get the relative path within wwwroot/img
                var relativePath = System.IO.Path.GetRelativePath(wwwRootPath, filePath);

                if (File.Exists(filePath))
                {
                    return filePath;
                }

                // Create an avatar image with circles
                using (var image = new Image<Rgba32>(100, 100))
                {
                    // Fill background with a random color
                    image.Mutate(ctx => ctx.BackgroundColor(selectedColor));

                    // Draw the user's name in the center of the image
                    var font = SystemFonts.CreateFont("Arial", 50);
                    var textColor = Color.White;


                    image.Mutate(ctx => ctx.DrawText(avatarString, font, textColor, new PointF(30, 25)));

                    // Save the image to the specified file path
                    image.Save(filePath);

                    _url_avatar = relativePath;
                }

                return _url_avatar;
            }
            catch (Exception)
            {
                return null;
            }
            

        }

        
    }

}

