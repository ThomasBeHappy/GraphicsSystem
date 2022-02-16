using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsSystem.Data
{
    public class Fonts
    {
        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Fonts.zap-ext-light16.psf")]
        public static byte[] Font;
    }
}
