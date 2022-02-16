#define COSMOSDEBUG
using Cosmos.Debug.Kernel;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using GraphicsSystem.Core;
using GraphicsSystem.Data;
using GraphicsSystem.Graphic.Controls;
using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.PCI.Audio;

namespace GraphicsSystem.Apps
{
    class FileExplorer : Window
    {
        static char[] name = new char[] { 'F', 'i', 'l', 'e', ' ', 'E','x','p','l','o','r','e','r','\0' };
        static char[] returnButton = new char[] { '<', '-', '\0' };
        bool refresh = true;

        List<DirectoryEntry> directoryEntries;
        public List<char[]> folderNames = new List<char[]>();

        public string path = "ROOT";

        static Bitmap icon = FileSystemBitmaps.bitmap;

        List<Control> controls = new List<Control>();

        public bool recentlyChanged = false;

        private static int tick = 0;


        public FileExplorer(uint x, uint y, uint width, uint height) : base(name, x, y, width, height, icon)
        {
        }

        public override void Update()
        {
            int sec = RTC.Second;
            if (tick != sec)
            {
                tick = sec;
                recentlyChanged = false;
            }

            if (refresh)
            {
                if (path == "ROOT")
                {
                    folderNames.Clear();
                    directoryEntries = FileSystem.fs.GetVolumes();

                    foreach (var item in directoryEntries)
                    {
                        folderNames.Add(item.mName.ToCharArray());
                    }
                    refresh = false;

                    
                }else
                {
                    folderNames.Clear();
                    directoryEntries = FileSystem.fs.GetDirectoryListing(path);

                    foreach (var item in directoryEntries) { 
                        folderNames.Add(item.mName.ToCharArray());
                    }
                    refresh = false;
                }
                
                controls.Clear();

                // TODO fix this
                for (int i = 0; i < folderNames.Count; i++)
                {
                    //Graphics.DrawString(x + 120, (uint)(y + 35 + ((i * 50) + 10)), font, folderNames[i], Color.white);
                    if (directoryEntries[i].mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        Button button = new Button(this, 200, 50, 120, (uint)(35 + ((i * 60))), Color.white, Color.black, Color.gray160, Kernel.mainFont, folderNames[i], directoryEntries[i].mFullPath);
                        button.OnClick += ChangeDirectory;
                        controls.Add(button);
                    }
                }
                
                Button returnB = new Button(this, 20, 20, 0, 0, Color.white, Color.black, Color.gray160, Kernel.mainFont, returnButton, null);
                returnB.OnClick += FolderBack;
                controls.Add(returnB);
            }

            Graphics.Rectangle(x, y, x + 60,y + height, Color.black);
            Graphics.Rectangle(x, y, x + width, y + 20, Color.black);

            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Draw();
                if (Z_Index == 0)
                {
                    controls[i].Update();
                }
            }

            //Draw all folders/volumes
            for (int i = 0; i < folderNames.Count; i++)
            {
                if (directoryEntries[i].mEntryType == DirectoryEntryTypeEnum.Directory)
                {
                    Graphics.DrawBitmapFromData((int)(x + 65), (int)(y + 35 + (i * 60)), 50, 50, FileSystemBitmaps.bitmap, Color.black);
                }
                else if (directoryEntries[i].mEntryType == DirectoryEntryTypeEnum.File)
                {
                    Graphics.DrawBitmapFromData((int)(x + 65), (int)(y + 35 + (i * 60)), 50, 50, FileSystemBitmaps.fileBitmap, Color.black);
                    Graphics.DrawString(x + 120, (uint)(y + 35 + ((i * 60))), Kernel.mainFont, folderNames[i], Color.white);
                }
                else if (directoryEntries[i].mEntryType == DirectoryEntryTypeEnum.Unknown)
                {
                    Graphics.DrawString(x + 120, (uint)(y + 35 + ((i * 60))), Kernel.mainFont, folderNames[i], Color.white);
                }

                //Graphics.DrawString(x + 120, (uint)(y + 35 + ((i * 50) + 10)), font, folderNames[i], Color.white);
            }

        }

        public void ChangeDirectory(object sender, EventArgs e)
        {
            if (!recentlyChanged)
            {
                path = (string)((Button)sender).value;
                recentlyChanged = true;
                refresh = true;
            }
        }

        public void FolderBack(object sender, EventArgs e)
        {
            if (!recentlyChanged)
            {
                int lastIndex = path.LastIndexOf('\\');
                if (lastIndex == path.Length - 1)
                {
                    path = "ROOT";
                    recentlyChanged = true;
                    refresh = true;
                    return;
                }

                if (lastIndex == 2)
                {
                    path = @"0:\";
                    recentlyChanged = true;
                    refresh = true;
                    return;
                }

                path = path.Remove(lastIndex);
                recentlyChanged = true;
                refresh = true;
            }
        }


        public string String;

        private string ToString(char[] vs)
        {
            String = "";
            for (int i = 0; i < vs.Length; i++)
            {
                String += vs[i];
            }

            return String;
        }
    }
}
