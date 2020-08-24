using System;
using System.IO;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        public static VMWareSVGAII vMWareSVGAII;

        protected override void BeforeRun()
        {
            #region Debug
            /*
            CosmosVFS cosmosVFS = new CosmosVFS();
            VFSManager.RegisterVFS(cosmosVFS);

            Bitmap sb = new Bitmap(@"0:\sb.bmp");
            Bitmap bitmap = new Bitmap(@"0:\logo.bmp");

            vMWareSVGAII = new VMWareSVGAII();
            vMWareSVGAII.SetMode(640, 480);

            Graphics graphics = new Graphics(640, 480);

            graphics.Clear(Color.Red.ToArgb());
            graphics.Fill(-100, 280, 200, 200, Color.Blue.ToArgb());
            graphics.DrawImage(10, 330, sb);
            graphics.SetPixel(60, 80, Color.Black.ToArgb());
            graphics.DrawLine(10, 10, 50, 50, Color.Green.ToArgb());
            graphics.DrawRectangle(100, 100, 20, 20, Color.Black.ToArgb());
            graphics.DrawImageWithAlpha(150, 10, bitmap);
            graphics.DrawCircle(400, 300, 50, Color.Brown.ToArgb());
            graphics.DrawACSIIString(40, 55, "Hello World", Color.White.ToArgb());
            graphics.Flush(0, 0, vMWareSVGAII.width, vMWareSVGAII.height, vMWareSVGAII.Video_Memory);

            Graphics graphics1 = new Graphics(100, 100);
            graphics1.Clear(Color.Green.ToArgb());
            graphics1.Flush(300, 300, vMWareSVGAII.width, vMWareSVGAII.height, vMWareSVGAII.Video_Memory);

            Graphics graphics2 = new Graphics(300, 100);
            graphics2.CopyFromGraphics(graphics, 15, 380);
            graphics2.Blur(1);
            graphics2.Flush(500, 350, vMWareSVGAII.width, vMWareSVGAII.height, vMWareSVGAII.Video_Memory);

            vMWareSVGAII.Update(0, 0, vMWareSVGAII.width, vMWareSVGAII.height);
            */
            #endregion

            CosmosVFS cosmosVFS = new CosmosVFS();
            VFSManager.RegisterVFS(cosmosVFS);

            VMWareSVGAII vMWareSVGAII = new VMWareSVGAII();
            vMWareSVGAII.Disable();
            Console.WriteLine($"Video RAM: {vMWareSVGAII.Video_Memory}");

            FileInfo fileInfo = new FileInfo(@"0:\1.bmp");
            Console.WriteLine(fileInfo.fi)
        }

        protected override void Run()
        {
        }
    }
}
