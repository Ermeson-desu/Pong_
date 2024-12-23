using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Program 
{
    static void Main(string[]args)
    {
        GameWindowSettings settings = new GameWindowSettings();

        NativeWindowSettings nativeSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(800, 600),
            Title = "Pong",
            Flags = ContextFlags.Default,
            WindowBorder = WindowBorder.Fixed ,
            Profile = ContextProfile.Compatability
        };

        using( GameWindow window = new GameWindow(settings, nativeSettings))
        {
            float yJogador1 = 0;
            float yJogador2 = 0;
            float tamanhoDaBola = 20;
            float xBola= 0f;
            float yBola= 0f;
            float velocidadeBolaX = 0.1f;
            float velocidadeBolaY = 0.1f;


            float XdoJogador1()
            {
                return -nativeSettings.ClientSize.X/2 + LarguraJogadores()/2;
            }

            float XdoJogador2()
            {
                return nativeSettings.ClientSize.X/2 - LarguraJogadores()/2;
            }

            float LarguraJogadores()
            {
                return tamanhoDaBola;
            }
            float AlturaJogadores()
            {
                return 3 * tamanhoDaBola;
            }
            window.RenderFrame += (FrameEventArgs e) =>
            {
                window.Context.MakeCurrent();
                GL.Viewport(0,0,nativeSettings.ClientSize.X, nativeSettings.ClientSize.Y);
                Matrix4 projection = Matrix4.CreateOrthographic(nativeSettings.ClientSize.X,nativeSettings.ClientSize.Y,0.0f,1.0f);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projection);

                GL.Clear(ClearBufferMask.ColorBufferBit);

                //Bola
                DesenharRetangulo(xBola, yBola, tamanhoDaBola, tamanhoDaBola);
                // jogador da esquerda
                DesenharRetangulo(XdoJogador1(), yJogador1,LarguraJogadores(), AlturaJogadores());
                // jogador da direita
                DesenharRetangulo(XdoJogador2(), yJogador2,LarguraJogadores(), AlturaJogadores());

                window.SwapBuffers();
            };

            window.UpdateFrame += (FrameEventArgs e) =>
            {
                int rodada = 0;
                xBola += velocidadeBolaX;
                yBola += velocidadeBolaY;
                float ladoDireitoBola = xBola + tamanhoDaBola/2;
                float ladoEsquerdoBola = xBola - tamanhoDaBola/2;
                float TetoBola = yBola + tamanhoDaBola/2;
                float chaoBola = yBola - tamanhoDaBola/2;
                float ladoDireitoTela = nativeSettings.ClientSize.X/2;
                float ladoEsquerdoTela= -nativeSettings.ClientSize.X/2;
                float TetoTela = nativeSettings.ClientSize.Y/2;
                float chaoTela = -nativeSettings.ClientSize.Y/2;
            
                if ( ladoDireitoBola > XdoJogador2()- LarguraJogadores()/2
                     && chaoBola < yJogador2 + AlturaJogadores()/2
                     && TetoBola > yJogador2 - AlturaJogadores()/2)
                {
                    velocidadeBolaX = velocidadeBolaX + rodada;
                    velocidadeBolaX = -velocidadeBolaX;
                    rodada++;
                }
                if( ladoEsquerdoBola < XdoJogador1()+ LarguraJogadores()/2
                    && chaoBola < yJogador1 + AlturaJogadores()/2
                    && TetoBola > yJogador1 - AlturaJogadores()/2)
                {
                    velocidadeBolaX = velocidadeBolaX + rodada;
                    velocidadeBolaX = -velocidadeBolaX;
                    rodada++;
                }
                if (TetoBola > TetoTela)
                {
                    velocidadeBolaY = -velocidadeBolaY;
                }
                if(chaoBola < chaoTela)
                {
                    velocidadeBolaY = -velocidadeBolaY;
                }
                if(xBola < ladoEsquerdoTela || xBola > ladoDireitoTela)
                {
                    xBola = 0;
                    yBola = 0;
                }
            };

            window.KeyDown += (KeyboardKeyEventArgs e) =>
            {
                int velocidadeDoPlayer = 40;
                if(window.IsKeyDown(Keys.W))
                {
                    yJogador1 += velocidadeDoPlayer;
                }
                if(window.IsKeyDown(Keys.S))
                {
                    yJogador1 -= velocidadeDoPlayer;
                }
                if(window.IsKeyDown(Keys.Up))
                {
                    yJogador2 +=velocidadeDoPlayer;
                }
                if(window.IsKeyDown(Keys.Down))
                {
                    yJogador2-=velocidadeDoPlayer;
                }
            };
            window.Run();
        }

        void DesenharRetangulo(float x, float y,float largura, float altura)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-0.5f * largura + x,-0.5f * altura + y);
            GL.Vertex2(0.5f * largura + x,-0.5f * altura + y);
            GL.Vertex2(0.5f * largura + x,0.5f * altura + y);
            GL.Vertex2(-0.5f * largura + x,0.5f * altura + y);
            GL.End();
        }

    }
}