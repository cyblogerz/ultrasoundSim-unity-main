using Core.Display.Interfaces;
using Utils;

namespace Core.Interfaces
{
   public interface IImgSrc
   { 
      void RenderColorImageInBitmap(ref ColorBitmap bitmap);
      void AddPostProcessingEffect(IImagePostProcessor effect);
   }
}