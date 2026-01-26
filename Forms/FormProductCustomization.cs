using System;
using System.Drawing;
using System.Windows.Forms;
using OOAD_Project.Patterns;

namespace OOAD_Project
{
    /// <summary>
    /// Product Customization Dialog - Uses Decorator Pattern
    /// Allows customers to add extras, choose size, apply discounts
    /// </summary>
    public partial class FormProductCustomization : Form
    {
        private IProduct _product;
        private decimal _basePrice;

        // UI Controls
        private Label lblProductName;
        private Label lblBasePrice;
        private GroupBox gbExtras;
        private CheckBox chkExtraCheese;
        private CheckBox chkBacon;
        private CheckBox chkAvocado;
        private GroupBox gbSize;
        private RadioButton rbRegular;
        private RadioButton rbLarge;
        private RadioButton rbExtraLarge;
        private GroupBox gbDiscounts;
        private CheckBox chkStudentDiscount;
        private CheckBox chkSeniorDiscount;
        private CheckBox chkHappyHour;
        private CheckBox chkAddTax;
        private Label lblFinalPrice;
        private Label lblFinalDescription;
        private Button btnAdd;
        private Button btnCancel;

        public IProduct CustomizedProduct => _product;

        public FormProductCustomization(int productId, string productName, decimal basePrice, string category)
        {
            _basePrice = basePrice;
            _product = new BaseProduct(productId, productName, basePrice, category);

            InitializeCustomComponents();
            UpdatePreview();
        }

        private void InitializeCustomComponents()
        {
            // Form settings
            this.Text = "Customize Your Order";
            this.Width = 500;
            this.Height = 750;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int yPos = 20;

            // Product Name
            lblProductName = new Label
            {
                Text = _product.GetName(),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            this.Controls.Add(lblProductName);
            yPos += 35;

            // Base Price
            lblBasePrice = new Label
            {
                Text = $"Base Price: ${_basePrice:F2}",
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            this.Controls.Add(lblBasePrice);
            yPos += 35;

            // ========== EXTRAS GROUP ==========
            gbExtras = new GroupBox
            {
                Text = "Add Extras",
                Location = new Point(20, yPos),
                Width = 440,
                Height = 110
            };

            chkExtraCheese = new CheckBox
            {
                Text = "Extra Cheese (+$1.50)",
                Location = new Point(15, 25),
                Width = 200,
                Tag = "cheese"
            };
            chkExtraCheese.CheckedChanged += OnCustomizationChanged;

            chkBacon = new CheckBox
            {
                Text = "Bacon (+$2.00)",
                Location = new Point(15, 50),
                Width = 200,
                Tag = "bacon"
            };
            chkBacon.CheckedChanged += OnCustomizationChanged;

            chkAvocado = new CheckBox
            {
                Text = "Avocado (+$1.75)",
                Location = new Point(15, 75),
                Width = 200,
                Tag = "avocado"
            };
            chkAvocado.CheckedChanged += OnCustomizationChanged;

            gbExtras.Controls.AddRange(new Control[] { chkExtraCheese, chkBacon, chkAvocado });
            this.Controls.Add(gbExtras);
            yPos += 120;

            // ========== SIZE GROUP ==========
            gbSize = new GroupBox
            {
                Text = "Size",
                Location = new Point(20, yPos),
                Width = 440,
                Height = 110
            };

            rbRegular = new RadioButton
            {
                Text = "Regular (Standard)",
                Location = new Point(15, 25),
                Width = 200,
                Checked = true,
                Tag = "regular"
            };
            rbRegular.CheckedChanged += OnCustomizationChanged;

            rbLarge = new RadioButton
            {
                Text = "Large (+$2.00)",
                Location = new Point(15, 50),
                Width = 200,
                Tag = "large"
            };
            rbLarge.CheckedChanged += OnCustomizationChanged;

            rbExtraLarge = new RadioButton
            {
                Text = "Extra Large (+$3.50)",
                Location = new Point(15, 75),
                Width = 200,
                Tag = "xl"
            };
            rbExtraLarge.CheckedChanged += OnCustomizationChanged;

            gbSize.Controls.AddRange(new Control[] { rbRegular, rbLarge, rbExtraLarge });
            this.Controls.Add(gbSize);
            yPos += 120;

            // ========== DISCOUNTS GROUP ==========
            gbDiscounts = new GroupBox
            {
                Text = "Discounts",
                Location = new Point(20, yPos),
                Width = 440,
                Height = 110
            };

            chkStudentDiscount = new CheckBox
            {
                Text = "Student Discount (-15%)",
                Location = new Point(15, 25),
                Width = 200,
                Tag = "student"
            };
            chkStudentDiscount.CheckedChanged += OnCustomizationChanged;

            chkSeniorDiscount = new CheckBox
            {
                Text = "Senior Citizen (-20%)",
                Location = new Point(15, 50),
                Width = 200,
                Tag = "senior"
            };
            chkSeniorDiscount.CheckedChanged += OnCustomizationChanged;

            chkHappyHour = new CheckBox
            {
                Text = "Happy Hour (-10%)",
                Location = new Point(15, 75),
                Width = 200,
                Tag = "happy"
            };
            chkHappyHour.CheckedChanged += OnCustomizationChanged;

            gbDiscounts.Controls.AddRange(new Control[] { chkStudentDiscount, chkSeniorDiscount, chkHappyHour });
            this.Controls.Add(gbDiscounts);
            yPos += 120;

            // ========== TAX ==========
            chkAddTax = new CheckBox
            {
                Text = "Add Tax (10%)",
                Location = new Point(35, yPos),
                Width = 200,
                Checked = true
            };
            chkAddTax.CheckedChanged += OnCustomizationChanged;
            this.Controls.Add(chkAddTax);
            yPos += 35;

            // ========== PREVIEW ==========
            lblFinalDescription = new Label
            {
                Text = "",
                Location = new Point(20, yPos),
                Width = 440,
                Height = 60,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(lblFinalDescription);
            yPos += 65;

            lblFinalPrice = new Label
            {
                Text = $"Total: ${_basePrice:F2}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Green,
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            this.Controls.Add(lblFinalPrice);
            yPos += 45;

            // ========== BUTTONS ==========
            btnAdd = new Button
            {
                Text = "Add to Order",
                Width = 150,
                Height = 40,
                Location = new Point(20, yPos),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(btnAdd);

            btnCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 40,
                Location = new Point(180, yPos),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);
        }

        private void OnCustomizationChanged(object? sender, EventArgs e)
        {
            UpdatePreview();
        }

        /// <summary>
        /// ✅ DECORATOR PATTERN IN ACTION
        /// Builds the product by stacking decorators based on user selections
        /// </summary>
        private void UpdatePreview()
        {
            // Start fresh with base product
            _product = new BaseProduct(
                _product.GetProductId(),
                lblProductName.Text,
                _basePrice,
                _product.GetCategory()
            );

            // ✅ Apply Extra Cheese Decorator
            if (chkExtraCheese.Checked)
            {
                _product = new ExtraCheeseDecorator(_product);
            }

            // ✅ Apply Topping Decorators
            if (chkBacon.Checked)
            {
                _product = new ExtraToppingDecorator(_product, "Bacon", 2.00m);
            }

            if (chkAvocado.Checked)
            {
                _product = new ExtraToppingDecorator(_product, "Avocado", 1.75m);
            }

            // ✅ Apply Size Decorator
            if (rbLarge.Checked)
            {
                _product = new LargeSizeDecorator(_product);
            }
            else if (rbExtraLarge.Checked)
            {
                _product = new ExtraLargeSizeDecorator(_product);
            }

            // ✅ Apply Discount Decorators (only one at a time)
            if (chkStudentDiscount.Checked)
            {
                _product = new DiscountDecorator(_product, 15, "Student Discount");
                chkSeniorDiscount.Checked = false;
                chkHappyHour.Checked = false;
            }
            else if (chkSeniorDiscount.Checked)
            {
                _product = new DiscountDecorator(_product, 20, "Senior Citizen");
                chkStudentDiscount.Checked = false;
                chkHappyHour.Checked = false;
            }
            else if (chkHappyHour.Checked)
            {
                _product = new DiscountDecorator(_product, 10, "Happy Hour");
                chkStudentDiscount.Checked = false;
                chkSeniorDiscount.Checked = false;
            }

            // ✅ Apply Tax Decorator (always last)
            if (chkAddTax.Checked)
            {
                _product = new TaxDecorator(_product);
            }

            // Update UI with decorated product info
            lblFinalDescription.Text = _product.GetDescription();
            lblFinalPrice.Text = $"Total: ${_product.GetPrice():F2}";
        }
    }
}