using System.Media;
using System.Windows.Forms;

public partial class Jumpscare : Form
{
    private bool realClose = false;
    private SoundPlayer player;
    private int jeffState = 0;
    private int shibaState = 0;
    private int colorState = 0;
    private int flashState = 0;

    public Jumpscare()
    {
        InitializeComponent();
    }

    private void Jumpscare_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!realClose)
        {
            e.Cancel = true;
        }
    }

    private void Jumpscare_Load(object sender, System.EventArgs e)
    {
        Size = new System.Drawing.Size(Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);
        pictureBox1.Size = new System.Drawing.Size(Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

        if (label1.Text == "Jeff The Killer")
        {
            pictureBox1.Image = Stub.Properties.Resources.jeff;
        }
        else if (label1.Text == "Flashing Jeff The Killer")
        {
            pictureBox1.Image = Stub.Properties.Resources.jeff1;
            timer2.Start();
        }
        else if (label1.Text == "The Exorcist 1")
        {
            pictureBox1.Image = Stub.Properties.Resources.witch;
        }
        else if (label1.Text == "The Exorcist 2")
        {
            pictureBox1.Image = Stub.Properties.Resources.exorcist;
        }
        else if (label1.Text == "Shiba Inu")
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba1;
            timer3.Start();
        }
        else if (label1.Text == "Flashing screen")
        {
            timer4.Start();
        }
        else if (label1.Text == "Annoying colors")
        {
            timer5.Start();
        }

        if (label1.Text != "Flashing screen" && label1.Text != "Annoying colors")
        {
            player = new SoundPlayer(Stub.Properties.Resources.jumpscare);
            player.PlayLooping();
        }

        if (label3.Text == "True" && label2.Text != "UNLIMITED_TIME")
        {
            timer1.Interval = int.Parse(label2.Text);
            timer1.Start();
        }
    }

    private void timer1_Tick(object sender, System.EventArgs e)
    {
        player.Stop();
        realClose = true;
        timer1.Stop();
        this.Close();
    }

    private void timer2_Tick(object sender, System.EventArgs e)
    {
        if (jeffState == 0)
        {
            pictureBox1.Image = Stub.Properties.Resources.jeff2;
            jeffState = 1;
        }
        else if (jeffState == 1)
        {
            pictureBox1.Image = Stub.Properties.Resources.jeff1;
            jeffState = 0;
        }
    }

    private void timer3_Tick(object sender, System.EventArgs e)
    {
        if (shibaState == 0)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 1;
        }
        else if (shibaState == 1)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba2;
            shibaState = 2;
        }
        else if (shibaState == 2)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 3;
        }
        else if (shibaState == 3)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba3;
            shibaState = 4;
        }
        else if (shibaState == 4)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 5;
        }
        else if (shibaState == 5)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba4;
            shibaState = 6;
        }
        else if (shibaState == 6)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 7;
        }
        else if (shibaState == 7)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba5;
            shibaState = 8;
        }
        else if (shibaState == 8)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 9;
        }
        else if (shibaState == 9)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba6;
            shibaState = 10;
        }
        else if (shibaState == 10)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 11;
        }
        else if (shibaState == 11)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba7;
            shibaState = 12;
        }
        else if (shibaState == 12)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 13;
        }
        else if (shibaState == 13)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba8;
            shibaState = 14;
        }
        else if (shibaState == 14)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 15;
        }
        else if (shibaState == 15)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba9;
            shibaState = 16;
        }
        else if (shibaState == 16)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 17;
        }
        else if (shibaState == 17)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba10;
            shibaState = 18;
        }
        else if (shibaState == 18)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba11;
            shibaState = 19;
        }
        else if (shibaState == 19)
        {
            pictureBox1.Image = Stub.Properties.Resources.shiba1;
            shibaState = 0;
        }
    }

    private void timer4_Tick(object sender, System.EventArgs e)
    {
        if (flashState == 0)
        {
            pictureBox1.BackColor = System.Drawing.Color.White;
            flashState = 1;
        }
        else
        {
            pictureBox1.BackColor = System.Drawing.Color.Black;
            flashState = 0;
        }
    }

    private void timer5_Tick(object sender, System.EventArgs e)
    {
        if (colorState == 0)
        {
            pictureBox1.BackColor = System.Drawing.Color.White;
            colorState = 1;
        }
        else if (colorState == 1)
        {
            pictureBox1.BackColor = System.Drawing.Color.Magenta;
            colorState = 2;
        }
        else if (colorState == 2)
        {
            pictureBox1.BackColor = System.Drawing.Color.Red;
            colorState = 3;
        }
        else if (colorState == 3)
        {
            pictureBox1.BackColor = System.Drawing.Color.Green;
            colorState = 4;
        }
        else if (colorState == 4)
        {
            pictureBox1.BackColor = System.Drawing.Color.Black;
            colorState = 0;
        }
    }
}