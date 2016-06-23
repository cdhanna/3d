using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;

namespace _3d
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        
        private Matrix viewMatrix, projectionMatrix, worldMatrix;
        private List<VertexPositionColor> points;
        private VertexBuffer vb;
        private IndexBuffer ib;


        private CubeVertexSet cube;
        private BasicEffect fx; // TODO replace this with a custom hlsl file.


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // create the camera position
            viewMatrix = Matrix.CreateLookAt(
                new Vector3(0.0f, 0.0f, 3.0f),  // position
                new Vector3(0, 0, 0),           // look at target
                new Vector3(0, 1, 0)            // "up"
                );

            // create the matrix that transforms the world to the 2d screen. I stole this from the internet.
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio, 0.01f, 100f);

            // create a global world position
            worldMatrix = Matrix.Identity;


            // create a vertex buffer, and populate it with 4 positions. 
            var vbData = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0, 0, .5f), Color.Red), // (0,0) top left
                new VertexPositionColor(new Vector3(1, 0, .5f), Color.White), // (1,0) top right
                new VertexPositionColor(new Vector3(1, 1, 0), Color.Blue), // (1,1) bot right
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Yellow), // (0,1) bot left
            };
            vb = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vbData.Length, BufferUsage.WriteOnly); 
            vb.SetData(vbData);

            // create an index buffer, and populate it data to draw a triangle from (0,0) to (1,0) to (1,1), and a second from (0,0) to (1,1) to (0,1)
            var ibData = new short[]
            {
                0,1,2, 0,2,3
            };
            ib = new IndexBuffer(GraphicsDevice, typeof(short), ibData.Length, BufferUsage.WriteOnly);
            ib.SetData(ibData);


            // create the basic effect used to interp the vertices on the graphics card. TODO, write our own. 
            fx = new BasicEffect(GraphicsDevice);
            fx.VertexColorEnabled = true;
            fx.View = viewMatrix;
            fx.World = worldMatrix;
            fx.Projection = projectionMatrix;
           
            cube = new CubeVertexSet();
            cube.Init(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            worldMatrix *= Matrix.CreateRotationY(.01f);
            fx.World = worldMatrix;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // set cull to none, otherwise we might not see shit. This is only for testing, in production, we'd be smart enough to know which way our polgons were facing.
            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            
            //GraphicsDevice.SetVertexBuffer(vb); // send the vertex buffer to the graphics card.
            //GraphicsDevice.Indices = ib; // send the index buffer to the graphics card (maybe?)
            //fx.LightingEnabled = true;

            foreach (EffectPass pass in fx.CurrentTechnique.Passes)
            {
                pass.Apply(); // sends pass to gfx. 
                
                cube.Draw(GraphicsDevice);
                // GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);      
            }
           
            base.Draw(gameTime);
        }
    }
}
