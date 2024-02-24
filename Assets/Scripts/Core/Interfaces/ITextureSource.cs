using UnityEngine;

namespace Core.Display.Interfaces
{
    public interface ITextureSource
    {
        void RenderNextFrame(ref Texture2D texture);
    }
}