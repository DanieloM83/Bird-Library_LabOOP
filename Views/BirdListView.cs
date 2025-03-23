using BirdLab.Models;
using BirdLab.Themes;

namespace BirdLab.Views
{

    public class BirdListView : UserControl
    {
        public event EventHandler<BirdDTO> BirdSelected;

        private readonly ListBox birdListBox = new();

        public BirdListView()
        {
            Dock = DockStyle.Fill;
            BackColor = CatppuccinMochaTheme.Crust;
            Padding = new Padding(5);
            BorderStyle = BorderStyle.FixedSingle;
            
            birdListBox.Dock = DockStyle.Fill;
            birdListBox.BackColor = CatppuccinMochaTheme.Mantle;
            birdListBox.ForeColor = CatppuccinMochaTheme.Text;
            birdListBox.Font = new Font("Inter", 14);
            birdListBox.BorderStyle = BorderStyle.None;
            birdListBox.ItemHeight = 40;
            birdListBox.IntegralHeight = false;
            birdListBox.DrawMode = DrawMode.OwnerDrawFixed;

            int hoverIndex = 0;
            birdListBox.DrawItem += (sender, e) => {
                if (e.Index < 0) return;

                var listBox = sender as ListBox;
                var itemText = listBox?.Items[e.Index].ToString();
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                bool isHovered = e.Index == hoverIndex;

                Color backgroundColor = isHovered ? CatppuccinMochaTheme.Surface0 : CatppuccinMochaTheme.Mantle;
                backgroundColor = isSelected ? CatppuccinMochaTheme.Surface2 : backgroundColor;
                Color textColor = CatppuccinMochaTheme.Text;

                using (Brush bgBrush = new SolidBrush(backgroundColor), textBrush = new SolidBrush(textColor))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                    e.Graphics.DrawString(itemText, e.Font, textBrush, e.Bounds.X + 10, e.Bounds.Y + 10);
                }
                
            };
            
            birdListBox.SelectedIndexChanged += (s, e) => OnBirdSelected();
            birdListBox.MouseMove += (s, e) => {
                int index = birdListBox.IndexFromPoint(e.Location);
                if (index != hoverIndex)
                { 
                    hoverIndex = index;
                    birdListBox.Invalidate();
                }
            };

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