using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOAD_Project.Components
{
    public partial class FoodCard : UserControl
    {
        public event EventHandler? OnSelect;

        private Categories _category;
        private double _price;

        public FoodCard()
        {
            InitializeComponent();

            // Make the whole card clickable: wire clicks recursively (single pass)
            this.Click += (s, e) => OnSelect?.Invoke(this, e);
            this.DoubleClick += (s, e) => OnSelect?.Invoke(this, e);

            // Recursively wire child clicks once
            WireChildClickEvents(this);
        }

        private void WireChildClickEvents(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                // Attach a handler that raises OnSelect once per click
                child.Click += (s, e) => OnSelect?.Invoke(this, e);
                child.DoubleClick += (s, e) => OnSelect?.Invoke(this, e);

                if (child.HasChildren) WireChildClickEvents(child);
            }
        }

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value ?? "Unnamed Item";
        }

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                lblPrice.Text = _price.ToString("C2"); // show currency on the card itself
            }
        }

        public Image Food
        {
            get => pbFood.Image;
            set
            {
                if (pbFood == null)
                {
                    MessageBox.Show("PictureBox not initialized. Check InitializeComponent().", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                pbFood.Image = value;
                pbFood.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        public enum Categories { MainDish, Appetizer, SideDish, Soup, Seafood, Dessert, Beverage }
        public Categories Category
        {
            get => _category;
            set => _category = value;
        }
    }
}
