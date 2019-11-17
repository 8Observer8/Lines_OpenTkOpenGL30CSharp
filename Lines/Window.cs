using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lines
{
    class Window : GameWindow
    {
        private int _program;
        private int _amountOfVertices;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Lines";
            Width = 256;
            Height = 256;

            GL.ClearColor(0.9f, 0.9f, 1f, 1f);

            _program = CreateShaderProgram();

            _amountOfVertices = InitVertexBuffers(_program);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.DrawArrays(PrimitiveType.Lines, 0, _amountOfVertices);

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        private int CreateShaderProgram()
        {
            string vertexShaderSource = @"
                #version 130

                in vec3 inPosition;

                void main()
                {
                    gl_Position = vec4(inPosition, 1.0);
                }
            ";

            string fragmentShaderSource = @"
                #version 130

                out vec4 outColor;

                void main()
                {
                    outColor = vec4(1.0, 0.0, 0.0, 1.0);
                }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            GL.UseProgram(program);

            return program;
        }

        private int InitVertexBuffers(int program)
        {
            float[] vertices = new float[]
            {
                0f, 0f, 0f,
                0.5f, 0.5f, 0f,
                -0.5f, -0.5f, 0f,
                0.5f, -0.5f, 0f
            };

            int vbo;
            GL.GenBuffers(1, out vbo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            int positionLocation = GL.GetAttribLocation(_program, "inPosition");
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(positionLocation);

            return vertices.Length / 3;
        }
    }
}
