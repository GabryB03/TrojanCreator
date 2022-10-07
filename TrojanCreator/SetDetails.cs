using MetroSuite;
using System.Windows.Forms;

public partial class SetDetails : MetroForm
{
    public SetDetails()
    {
        InitializeComponent();
    }

    private void gunaButton6_Click(object sender, System.EventArgs e)
    {
        if (MessageBox.Show("Are you sure you want to modify this instruction?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
        {
            Utils.instructions[int.Parse(label1.Text)].Name = gunaLineTextBox1.Text;
            Utils.instructions[int.Parse(label1.Text)].Details = gunaTextBox1.Text;
        }
    }
}