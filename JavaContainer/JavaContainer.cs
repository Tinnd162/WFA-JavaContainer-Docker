using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JavaContainer
{
    public partial class JavaContainer : Form
    {
        public JavaContainer()
        {
            InitializeComponent();
        }
        public string ExecuteCommandSync(object command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo prStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + command);
                prStartInfo.RedirectStandardOutput = true;
                prStartInfo.UseShellExecute = false;
                prStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = prStartInfo;
                proc.Start();
                string res = proc.StandardOutput.ReadToEnd();
                return res;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public void CreateFileCompile()
        {
            string PathCompile = @"C:/DockerFile/compile.sh";
            string ContentCompile = @"cd /data && javac Demo.java && java Demo " + " \"$@\"";
            File.WriteAllText(PathCompile, ContentCompile);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            ExecuteCommandSync("docker run -it -d -v C:/DockerFile:/data  --name javacompile ubuntu:18.04");
        }
        private void btnCompile_Click(object sender, EventArgs e)
        {

            string PathDemo = @"C:/DockerFile/Demo.java";
            File.WriteAllText(PathDemo, txtCode.Text);

            CreateFileCompile();

            string res = ExecuteCommandSync("docker exec javacompile bash /data/compile.sh" + "  " + txtInput.Text);
            MessageBox.Show(res, "Notification", MessageBoxButtons.OK);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ExecuteCommandSync("docker stop javacompile");
            ExecuteCommandSync("docker rm javacompile");
        }
    }
}
