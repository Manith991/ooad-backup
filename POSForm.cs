using Npgsql;
using OOAD_Project.Components;
using QRCoder;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OOAD_Project
{
    public partial class POSForm : Form
    {
        #region WinAPI for Dragging Form
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        #endregion

        private readonly string currentUser;
        private readonly string orderType;
        private readonly string? tableName;
        private readonly int? tableId;
        private int? orderId;
        private readonly Form? parentForm;

        public POSForm(Form? parent, string username, string orderType, string? tableName = null, int? tableId = null, int? orderId = null)
        {
            InitializeComponent();
            SetupDataGridView();
            AssignCategoryTags();

            parentForm = parent;
            currentUser = username;
            this.orderType = orderType;
            this.tableName = tableName;
            this.tableId = tableId;
            this.orderId = orderId;

            InitializeOrderDetails();
        }

        #region Initialization
        private void InitializeOrderDetails()
        {
            lblUser.Text = currentUser;
            lblOrderType.Text = orderType;

            lblTableName.Visible = orderType == "Dine-in" || orderType == "Eat Here" || orderType.Equals("Eat Here", StringComparison.OrdinalIgnoreCase);
            lblTableName.Text = tableName ?? "Unknown Table";
            if (orderId.HasValue)
                LoadExistingOrderDetails(); // ✅ Load items for taken tables
        }

        private void POSForm_Shown(object sender, EventArgs e)
        {
            LoadProductsFromDatabase();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.WindowState = FormWindowState.Maximized;
        }
        #endregion

        #region Load Products
        private void LoadProductsFromDatabase(string? categoryFilter = null)
        {
            flowLayoutPanel.Controls.Clear();
            flowLayoutPanel.Padding = new Padding(20);

            string query = @"
                SELECT p.productname, p.price, c.categoryname, p.imagepath
                FROM products p
                LEFT JOIN categories c ON p.categoryid = c.categoryid";

            if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Foods")
                query += $" WHERE c.categoryname = '{categoryFilter.Replace("'", "''")}'";

            DataTable dt = Database.GetData(query);

            if (dt.Rows.Count == 0)
            {
                flowLayoutPanel.Controls.Add(CreateNoProductLabel());
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                flowLayoutPanel.Controls.Add(CreateFoodCard(
                    row["productname"]?.ToString() ?? "Unnamed",
                    row["price"] == DBNull.Value ? 0.0 : Convert.ToDouble(row["price"]),
                    row["imagepath"]?.ToString() ?? ""
                ));
            }
        }

        private Label CreateNoProductLabel()
        {
            return new Label
            {
                Text = "No products found in this category.",
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private FoodCard CreateFoodCard(string name, double price, string imagePath)
        {
            Image? img = LoadImage(imagePath);

            var card = new FoodCard
            {
                Title = name,
                Price = price,
                Food = img,
                Margin = new Padding(10)
            };

            card.OnSelect += FoodCard_OnSelect;
            return card;
        }

        private Image? LoadImage(string path)
        {
            string[] pathsToCheck =
            {
                path,
                Path.Combine(Application.StartupPath, "Resources", "Foods", path),
                Path.Combine(Application.StartupPath, "Resources", "Foods", path + ".png"),
                Path.Combine(Application.StartupPath, "Resources", "Foods", path + ".jpg"),
                Path.Combine(Application.StartupPath, "Resources", "Foods", path + ".jpeg")
            };

            foreach (var p in pathsToCheck)
                if (File.Exists(p)) return Image.FromFile(p);

            string defaultImg = Path.Combine(Application.StartupPath, "Resources", "no_image.png");
            return File.Exists(defaultImg) ? Image.FromFile(defaultImg) : null;
        }
        #endregion

        #region Search
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search items...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search items...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search items...") return;

            foreach (FoodCard card in flowLayoutPanel.Controls)
            {
                card.Visible = card.Title.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
        #endregion

        #region DataGridView Helpers
        private void SetupDataGridView()
        {
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            dgvItems.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            dgvItems.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgvItems.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            dgvItems.RowsAdded += (s, e) => UpdateRowNumbers();
            dgvItems.RowsRemoved += (s, e) => UpdateRowNumbers();

            if (dgvItems.Columns["no"] != null)
                dgvItems.Columns["no"].ReadOnly = true;
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
                dgvItems.Rows[i].Cells["no"].Value = (i + 1).ToString();
        }

        private void FoodCard_OnSelect(object? sender, EventArgs e)
        {
            if (sender is not FoodCard card) return;

            int productId = GetProductIdByName(card.Title);
            if (productId == -1)
            {
                MessageBox.Show("Product not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if item exists in DataGridView
            DataGridViewRow? existingRow = dgvItems.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["item"].Value?.ToString() == card.Title);

            if (existingRow != null)
            {
                int qty = Convert.ToInt32(existingRow.Cells["quantity"].Value) + 1;
                existingRow.Cells["quantity"].Value = qty;
                existingRow.Cells["total"].Value = qty * card.Price;
                UpdateOrderDetail(productId, qty, card.Price); // ✅ Update DB
            }
            else
            {
                int rowIdx = dgvItems.Rows.Add();
                var newRow = dgvItems.Rows[rowIdx];
                newRow.Cells["item"].Value = card.Title;
                newRow.Cells["price"].Value = card.Price;
                newRow.Cells["quantity"].Value = 1;
                newRow.Cells["total"].Value = card.Price;
                InsertOrderDetail(productId, 1, card.Price); // ✅ Insert DB
            }
            UpdateRowNumbers();
            UpdateGrandTotal();
            UpdateOrderTotal();
        }

        private void UpdateGrandTotal()
        {
            double sum = dgvItems.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["total"].Value != null)
                .Sum(r => Convert.ToDouble(r.Cells["total"].Value));

            lbTotalPrice.Text = sum.ToString("C2");
        }

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Columns[e.ColumnIndex].Name != "quantity") return;

            var row = dgvItems.Rows[e.RowIndex];
            if (double.TryParse(row.Cells["price"].Value?.ToString(), out double price) &&
                int.TryParse(row.Cells["quantity"].Value?.ToString(), out int qty))
            {
                row.Cells["total"].Value = price * qty;
            }
            else
            {
                row.Cells["quantity"].Value = 1;
                row.Cells["total"].Value = price;
            }
            UpdateGrandTotal();
        }

        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["quantity"].Index)
            {
                e.Control.KeyPress -= Quantity_KeyPress;
                e.Control.KeyPress += Quantity_KeyPress;
            }
        }

        private void Quantity_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        #endregion

        #region Remove / Clear
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in dgvItems.SelectedRows)
                dgvItems.Rows.Remove(row);

            UpdateGrandTotal();
            UpdateRowNumbers();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("There are no items to clear.", "Empty List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to clear all items?", "Confirm Clear All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dgvItems.Rows.Clear();
                UpdateGrandTotal();
            }
        }
        #endregion

        #region Payments
        private void btnPay_Click(object sender, EventArgs e) => ProcessPayment("Cash");
        private void btnPayQR_Click(object sender, EventArgs e) => ProcessPayment("QR");

        private void ProcessPayment(string paymentMethod)
        {
            double total = dgvItems.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["total"].Value != null)
                .Sum(r => Convert.ToDouble(r.Cells["total"].Value));

            if (total <= 0)
            {
                MessageBox.Show("No items to pay for.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (paymentMethod == "QR")
            {
                ShowQRPayment(total);
                return;
            }
            CompletePayment(total, paymentMethod);
        }

        private void ShowQRPayment(double total)
        {
            string qrContent = $"PAYMENT|Method=QR|Amount={total:F2}|Date={DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrData);
            Bitmap qrImage = qrCode.GetGraphic(30);

            Form qrForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Scan to Pay (QR)",
                ClientSize = new Size(600, 650)
            };

            PictureBox pb = new PictureBox { Dock = DockStyle.Fill, Image = qrImage, SizeMode = PictureBoxSizeMode.Zoom };
            Button btnConfirm = new Button
            {
                Text = "Confirm Payment",
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnConfirm.Click += (s, e) =>
            {
                qrForm.Close();
                CompletePayment(total, "QR");
            };

            qrForm.Controls.Add(pb);
            qrForm.Controls.Add(btnConfirm);
            qrForm.ShowDialog();
        }

        private void CompletePayment(double total, string method)
        {
            try
            {
                int userId = GetUserIdByUsername(currentUser);
                if (userId == -1) return;

                using var conn = Database.GetConnection();
                conn.Open();
                using var tx = conn.BeginTransaction();

                if (orderId.HasValue)
                {
                    UpdateExistingOrder(conn, tx, total, method);
                    if (tableId.HasValue) UpdateTableStatus(conn, tx, tableId.Value, "Available");
                }
                else
                {
                    InsertNewOrder(conn, tx, userId, total, method);
                }

                tx.Commit();

                MessageBox.Show($"{method} Payment successful.", "Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvItems.Rows.Clear();
                UpdateGrandTotal();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{method} Payment error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateExistingOrder(NpgsqlConnection conn, NpgsqlTransaction tx, double total, string method)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = @"
                UPDATE orders
                SET total_amount = @total, status = 'Paid', payment_method = @method
                WHERE order_id = @oid";
            cmd.Parameters.AddWithValue("total", total);
            cmd.Parameters.AddWithValue("method", method);
            cmd.Parameters.AddWithValue("oid", orderId.Value);
            cmd.ExecuteNonQuery();
        }

        private void InsertNewOrder(NpgsqlConnection conn, NpgsqlTransaction tx, int userId, double total, string method)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = @"
                INSERT INTO orders (table_id, user_id, order_type, order_date, total_amount, status, payment_method)
                VALUES (NULL, @uid, 'Takeaway', NOW(), @total, 'Paid', @method)";
            cmd.Parameters.AddWithValue("uid", userId);
            cmd.Parameters.AddWithValue("total", total);
            cmd.Parameters.AddWithValue("method", method);
            cmd.ExecuteNonQuery();
        }

        private void UpdateTableStatus(NpgsqlConnection conn, NpgsqlTransaction tx, int tableId, string status)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "UPDATE tables SET status = @status WHERE table_id = @tid";
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("tid", tableId);
            cmd.ExecuteNonQuery();
        }

        private int GetUserIdByUsername(string username)
        {
            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT id FROM users WHERE username=@un LIMIT 1", conn);
            cmd.Parameters.AddWithValue("un", username);
            conn.Open();

            var result = cmd.ExecuteScalar();
            if (result == null)
            {
                MessageBox.Show("User not found: " + username, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            return Convert.ToInt32(result);
        }
        #endregion

        #region Categories
        private void btnAllFood_Click(object sender, EventArgs e) => LoadProductsFromDatabase("All Foods");
        private void btnMainDish_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Main Dishes");
        private void btnAppetizer_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Appetizers");
        private void btnSideDish_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Side Dishes");
        private void btnSoup_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Soups/Salads");
        private void btnSeafood_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Seafood");
        private void btnBeverage_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Beverages");
        private void btnDessert_Click(object sender, EventArgs e) => LoadProductsFromDatabase("Desserts");
        #endregion

        #region Misc
        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
            parentForm?.Show();
        }
        #endregion

        private void LoadExistingOrderDetails()
        {
            if (!orderId.HasValue) return;

            string query = @"
        SELECT p.productname, od.quantity, od.price, od.subtotal
        FROM order_details od
        JOIN products p ON od.product_id = p.productid
        WHERE od.order_id = @oid;";

            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("oid", orderId.Value);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int index = dgvItems.Rows.Add();
                var row = dgvItems.Rows[index];
                row.Cells["item"].Value = reader.GetString(0);
                row.Cells["quantity"].Value = reader.GetInt32(1);
                row.Cells["price"].Value = Convert.ToDouble(reader.GetDecimal(2));
                row.Cells["total"].Value = Convert.ToDouble(reader.GetDecimal(3));
            }

            UpdateRowNumbers();
            UpdateGrandTotal();
        }
        private int GetProductIdByName(string name)
        {
            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT productid FROM products WHERE productname = @n LIMIT 1", conn);
            cmd.Parameters.AddWithValue("n", name);
            conn.Open();
            var result = cmd.ExecuteScalar();
            return result == null ? -1 : Convert.ToInt32(result);
        }

        private void InsertOrderDetail(int productId, int quantity, double price)
        {
            // If we don't have an order yet, create one and capture its id
            if (!orderId.HasValue)
            {
                try
                {
                    using var conn = Database.GetConnection();
                    conn.Open();

                    using var cmd = conn.CreateCommand();
                    // use parameters for safety; tableId may be null
                    cmd.CommandText = @"
                INSERT INTO orders (table_id, user_id, order_type, order_date, total_amount, status, payment_method)
                VALUES (@tid, @uid, @otype, NOW(), 0, 'Pending', 'Cash')
                RETURNING order_id;
            ";

                    // user id from currentUser (if you need a user id here, get it)
                    int uid = GetUserIdByUsername(currentUser);
                    if (uid == -1) return;

                    if (tableId.HasValue)
                        cmd.Parameters.AddWithValue("tid", tableId.Value);
                    else
                        cmd.Parameters.AddWithValue("tid", DBNull.Value);

                    cmd.Parameters.AddWithValue("uid", uid);
                    cmd.Parameters.AddWithValue("otype", orderType ?? "Takeaway");

                    var newIdObj = cmd.ExecuteScalar();
                    if (newIdObj != null)
                        orderId = Convert.ToInt32(newIdObj);
                    else
                        throw new Exception("Failed to create order record.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // Now we have an orderId — insert/update order_details
            try
            {
                string query = @"
            INSERT INTO order_details (order_id, product_id, quantity, price)
            VALUES (@oid, @pid, @qty, @price)
            ON CONFLICT (order_id, product_id) DO UPDATE 
                SET quantity = order_details.quantity + EXCLUDED.quantity,
                    price = EXCLUDED.price;
        ";

                using var conn = Database.GetConnection();
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("oid", orderId.Value);
                cmd.Parameters.AddWithValue("pid", productId);
                cmd.Parameters.AddWithValue("qty", quantity);
                cmd.Parameters.AddWithValue("price", Convert.ToDecimal(price));
                conn.Open();
                cmd.ExecuteNonQuery();

                // Keep order totals updated in-memory UI
                UpdateOrderTotal(); // updates orders.total_amount from order_details (you already have this)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving order item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOrderDetail(int productId, int newQty, double price)
        {
            if (!orderId.HasValue) return;

            string query = "UPDATE order_details SET quantity=@q, price=@p WHERE order_id=@oid AND product_id=@pid";
            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("q", newQty);
            cmd.Parameters.AddWithValue("p", price);
            cmd.Parameters.AddWithValue("oid", orderId.Value);
            cmd.Parameters.AddWithValue("pid", productId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private void UpdateOrderTotal()
        {
            if (!orderId.HasValue) return;

            string query = "UPDATE orders SET total_amount = (SELECT SUM(subtotal) FROM order_details WHERE order_id=@oid) WHERE order_id=@oid";
            using var conn = Database.GetConnection();
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("oid", orderId.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private void AssignCategoryTags()
        {
            string query = "SELECT categoryid, categoryname FROM categories ORDER BY categoryid ASC";
            DataTable dt = Database.GetData(query);

            int i = 0;
            var buttons = new[]
            {
        btnAllFood, btnMainDish, btnAppetizer, btnSideDish,
        btnSoup, btnSeafood, btnBeverage, btnDessert
    };

            foreach (DataRow row in dt.Rows)
            {
                if (i >= buttons.Length) break;

                int id = Convert.ToInt32(row["categoryid"]);
                string name = row["categoryname"].ToString();

                buttons[i].Tag = id;        // ✅ use categoryid instead of name
                buttons[i].Text = name;
                i++;
            }
        }

        public void RefreshCategoryButtons()
        {
            string query = "SELECT categoryid, categoryname, imagepath FROM categories";
            DataTable dt = Database.GetData(query);

            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["categoryid"]);
                string name = row["categoryname"].ToString();
                string imgPath = row["imagepath"].ToString();

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn && btn.Tag is int tagId && tagId == id)
                    {
                        // Update text
                        btn.Text = name;

                        // Update image
                        string fullPath = Path.Combine(Application.StartupPath, "Resources", imgPath);
                        if (File.Exists(fullPath))
                        {
                            btn.Image?.Dispose();
                            btn.Image = Image.FromFile(fullPath);
                        }
                    }
                }
            }
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            using (var form = new CategoriesForm("(admin)"))
            {
                if (form.ShowDialog() == DialogResult.OK)   // Only executes if CategoriesForm sets DialogResult = OK
                {
                    RefreshCategoryButtons();               // Refresh buttons now that category was updated
                }
            }

        }
    }
}
