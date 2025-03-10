using BirdLab.Models;

namespace BirdLab.Views
{

    public class BirdListView : UserControl
    {
        public event EventHandler<BirdDTO> BirdSelected;

        private readonly ListBox birdListBox = new ListBox();

        public BirdListView()
        {
            BackColor = Color.White;
            Padding = new Padding(15);
            Size = new Size(300, 400);
            BorderStyle = BorderStyle.FixedSingle;

            birdListBox.Dock = DockStyle.Fill;
            birdListBox.Font = new Font("Segoe UI", 12);
            birdListBox.ForeColor = Color.Black;
            birdListBox.BackColor = Color.White;
            birdListBox.BorderStyle = BorderStyle.None;
            birdListBox.ItemHeight = 40;
            birdListBox.IntegralHeight = false;
            birdListBox.DrawMode = DrawMode.OwnerDrawFixed;
            birdListBox.DrawItem += (sender, e) => {
                if (e.Index < 0) return;

                var listBox = sender as ListBox;
                var itemText = listBox.Items[e.Index].ToString();
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                Color backgroundColor = isSelected ? Color.DeepSkyBlue : Color.White;
                Color textColor = isSelected ? Color.White : Color.Black;

                using (Brush bgBrush = new SolidBrush(backgroundColor), textBrush = new SolidBrush(textColor))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                    e.Graphics.DrawString(itemText, e.Font, textBrush, e.Bounds.X + 10, e.Bounds.Y + 10);
                }

                using Pen borderPen = new(Color.LightGray, 1);
                e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            };
            
            birdListBox.SelectedIndexChanged += (s, e) => OnBirdSelected();

            Controls.Add(birdListBox);
        }
        

        private void OnBirdSelected()
        {
            if (birdListBox.SelectedItem is BirdDTO selectedBird)
            {
                BirdSelected?.Invoke(this, selectedBird);
            }
        }

        public void UpdateBirdList(List<BirdDTO> birds)
        {
            birdListBox.Items.Clear();
            birdListBox.Items.AddRange([.. birds]);
        }
    }
}