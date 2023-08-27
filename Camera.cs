using Microsoft.Xna.Framework;
using System;

namespace _2D_Camera
{
    public class Camera
    {
        // no scaling
        public Camera()
        {
            Zoom = 1.0f;    
        }

        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }             // 1.0f standart
        public float Rotation { get; private set; }         // 0.0f standart

        // Viewport needs to be adjusted any time the player resizes the game window
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(ViewportWidth / 2f, ViewportHeight / 2f);
            }
        }
        
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                                                Matrix.CreateRotationZ(Rotation) *
                                                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                                Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        // call this method with negative values to zoom out or positive values to zoom in
        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if(Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }

        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Vector2 newPosition = Position + cameraMovement;

            if(clampToMap)
            {
                Position = MapClampedPosition(newPosition);
            }
            else
            {
                Position = newPosition;
            }
        }

        public Rectangle ViewportWorldBoundry()
        {
            Vector2 viewportCorner = ScreenToWorld(Vector2.Zero);
            Vector2 viewportButtomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewportCorner.X, (int)viewportCorner.Y,
                                 (int)(viewportButtomCorner.X - viewportCorner.X),
                                 (int)(viewportButtomCorner.Y - viewportCorner.Y));
        }

        // center the camera on specific coordinates
        public void CenterOn(Vector2 position)
        {
            Position = position;
        }

        private Vector2 MapClampedPosition(Vector2 position)
        {
            var cameraMax = new Vector2(672 * 64 -
                                        (ViewportWidth / Zoom / 2),
                                        512 * 64 -
                                        (ViewportHeight / Zoom / 2));

            return Vector2.Clamp(position,
               new Vector2(ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2),
               cameraMax);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        private Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }

        public void HandleInput()
        {
            Vector2 cameraMovement = Vector2.Zero;

            if (InputState.IsScrollLeft())
            {
                cameraMovement.X = -1;
            }
            else if (InputState.IsScrollRight())
            {
                cameraMovement.X = 1;
            }
            if (InputState.IsScrollUp())
            {
                cameraMovement.Y = -1;
            }
            else if (InputState.IsScrollDown())
            {
                cameraMovement.Y = 1;
            }
            if (InputState.IsZoomIn())
            {
                AdjustZoom(0.25f);
            }
            else if (InputState.IsZoomOut())
            {
                AdjustZoom(-0.25f);
            }

            // When using a controller, to match the thumbstick behavior,
            // we need to normalize non-zero vectors in case the user
            // is pressing a diagonal direction.
            if (cameraMovement != Vector2.Zero)
            {
                cameraMovement.Normalize();
            }

            // scale our movement to move 25 pixels per second
            cameraMovement *= 25f;

            MoveCamera(cameraMovement, true);
        }
    }
}
