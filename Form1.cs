using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenGL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Size = new Size(600,600);
        }

        int RayTracingProgramID;
        int RayTracingVertexShader;
        int RayTracingFragmentShader;

        void loadShader(List<String> filenames, ShaderType type, int program, out int address)
        {

            address = GL.CreateShader(type);
            String shaderText = "";
            for (int i = 0; i < filenames.Count; i++)
            {
                System.IO.StreamReader sr = new StreamReader(filenames[i]);
                shaderText += sr.ReadToEnd();
            }
            GL.ShaderSource(address, shaderText);
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private bool InitShaders()
        {
            RayTracingProgramID = GL.CreateProgram();
            List<String> vertexShaderText = new List<string>();
            vertexShaderText.Add("..\\..\\Shaders\\raytracing.vert");
            List<String> fragmentShaderText = new List<string>();
            fragmentShaderText.Add("..\\..\\Shaders\\raytracing.frag");

            loadShader(vertexShaderText, ShaderType.VertexShader, RayTracingProgramID, out RayTracingVertexShader);
            loadShader(fragmentShaderText, ShaderType.FragmentShader, RayTracingProgramID, out RayTracingFragmentShader);

            GL.LinkProgram(RayTracingProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(RayTracingProgramID));
            GL.Enable(EnableCap.Texture2D);
            return true;
        }


        private static bool Init()
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            return true;
        }

        private static void Resize(int width, int height)
        {
            if (height == 0)
            {
                height = 1;
            }
            GL.Viewport(0, 0, width, height);
        }


        private void Draw()
        {
            GL.ClearColor(Color.AliceBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.UseProgram(RayTracingProgramID);
       
        
           
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 1);

            GL.End();
            openGlControl.SwapBuffers();
            GL.UseProgram(0);

        }

        private void openGlControl_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void openGlControl_Load(object sender, EventArgs e)
        {
            Init();
            Resize(openGlControl.Width, openGlControl.Height);
            InitShaders();
        }


        private void openGlControl_Resize(object sender, EventArgs e)
        {
            Resize(openGlControl.Width, openGlControl.Height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            openGlControl.Width = this.ClientRectangle.Width - 19;
            openGlControl.Height = this.ClientRectangle.Height -19;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }

   

}

