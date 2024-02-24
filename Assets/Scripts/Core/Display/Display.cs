using System;
using Core.Display.Interfaces;

namespace Core.Display
{
    using UnityEngine;
    using System.Collections;
    


// manages the display behaviour
    public class Display : MonoBehaviour{
        private Texture2D _texture;
        private ITextureSource _textureSource;

        public enum DisplayModes
        {
            RayCast,
            InvR,
            Fake
        };

        public DisplayModes displayMode;
        public int width = 640;
        public int height = 480;
        private Renderer _renderer;
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _texture = new Texture2D(width,height,TextureFormat.RGB24,false);
            _renderer.material.mainTexture = _texture;
        }

        private void Update()
        {
            _textureSource.RenderNextFrame(ref _texture);
        }

        private ITextureSource switchMode()
        {
            return null;
        }
    }
}