// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

using System.IO;

namespace MyDownloader.Core
{
    public interface IProtocolProvider
    {
        Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition);

        RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream? stream);
    }
}