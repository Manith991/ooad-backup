using System;
using System.Windows.Forms;
using OOAD_Project.Domain;

namespace OOAD_Project.Patterns
{
    /// <summary>
    /// STRATEGY PATTERN IMPLEMENTATION
    /// Defines a family of payment algorithms, encapsulates each one,
    /// and makes them interchangeable at runtime.
    /// </summary>

    #region Strategy Interface

    /// <summary>
    /// Strategy interface - all payment methods must implement this
    /// </summary>
    public interface IPaymentStrategy
    {
        /// <summary>
        /// Process payment and return success/failure
        /// </summary>
        bool ProcessPayment(decimal amount, Order order);

        /// <summary>
        /// Get the name of this payment method
        /// </summary>
        string GetPaymentMethodName();

        /// <summary>
        /// Print/display receipt after successful payment
        /// </summary>
        void PrintReceipt(Order order);

        /// <summary>
        /// Validate if payment can be processed
        /// </summary>
        bool CanProcess(decimal amount);
    }

    #endregion

    #region Concrete Strategy Implementations

    /// <summary>
    /// Cash payment strategy
    /// </summary>
    public class CashPaymentStrategy : IPaymentStrategy
    {
        private decimal _cashReceived;
        private decimal _change;

        public bool ProcessPayment(decimal amount, Order order)
        {
            // Show cash input dialog
            using (var cashDialog = new Form())
            {
                cashDialog.Text = "Cash Payment";
                cashDialog.Width = 400;
                cashDialog.Height = 250;
                cashDialog.StartPosition = FormStartPosition.CenterParent;
                cashDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                cashDialog.MaximizeBox = false;
                cashDialog.MinimizeBox = false;

                // Amount label
                var lblAmount = new Label
                {
                    Text = $"Total Amount: ${amount:F2}",
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                    Location = new System.Drawing.Point(20, 20)
                };

                // Cash received label
                var lblCash = new Label
                {
                    Text = "Cash Received:",
                    AutoSize = true,
                    Location = new System.Drawing.Point(20, 60)
                };

                // Cash input textbox
                var txtCash = new TextBox
                {
                    Width = 200,
                    Location = new System.Drawing.Point(140, 57),
                    Font = new System.Drawing.Font("Segoe UI", 10)
                };

                // Change label
                var lblChange = new Label
                {
                    Text = "Change: $0.00",
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold),
                    ForeColor = System.Drawing.Color.Green,
                    Location = new System.Drawing.Point(20, 100)
                };

                // Calculate change on text change
                txtCash.TextChanged += (s, e) =>
                {
                    if (decimal.TryParse(txtCash.Text, out decimal cash))
                    {
                        decimal change = cash - amount;
                        lblChange.Text = $"Change: ${change:F2}";
                        lblChange.ForeColor = change >= 0
                            ? System.Drawing.Color.Green
                            : System.Drawing.Color.Red;
                    }
                };

                // Confirm button
                var btnConfirm = new Button
                {
                    Text = "Confirm Payment",
                    Width = 150,
                    Height = 40,
                    Location = new System.Drawing.Point(20, 140),
                    DialogResult = DialogResult.OK
                };

                btnConfirm.Click += (s, e) =>
                {
                    if (decimal.TryParse(txtCash.Text, out decimal cash))
                    {
                        if (cash >= amount)
                        {
                            _cashReceived = cash;
                            _change = cash - amount;
                            cashDialog.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("Insufficient cash amount!", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid amount!", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // Cancel button
                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Width = 100,
                    Height = 40,
                    Location = new System.Drawing.Point(180, 140),
                    DialogResult = DialogResult.Cancel
                };

                cashDialog.Controls.AddRange(new Control[] {
                    lblAmount, lblCash, txtCash, lblChange, btnConfirm, btnCancel
                });

                if (cashDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show(
                        $"Payment successful!\n\nTotal: ${amount:F2}\nCash: ${_cashReceived:F2}\nChange: ${_change:F2}",
                        "Cash Payment Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return true;
                }
            }

            return false;
        }

        public string GetPaymentMethodName() => "Cash";

        public void PrintReceipt(Order order)
        {
            string receipt = $@"
========================================
           RESTAURANT POS RECEIPT
========================================
Order ID:        {order.OrderId}
Date:            {DateTime.Now:yyyy-MM-dd HH:mm:ss}
Table:           {order.TableId?.ToString() ?? "Takeaway"}
----------------------------------------
Total Amount:    ${order.TotalAmount:F2}
Payment Method:  Cash
Cash Received:   ${_cashReceived:F2}
Change:          ${_change:F2}
========================================
        Thank you for your business!
========================================
";
            Console.WriteLine(receipt);

            // In real app: send to printer or save as PDF
            MessageBox.Show(receipt, "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool CanProcess(decimal amount) => amount > 0;
    }

    /// <summary>
    /// QR Code payment strategy
    /// </summary>
    public class QRPaymentStrategy : IPaymentStrategy
    {
        private string? _transactionId;

        public bool ProcessPayment(decimal amount, Order order)
        {
            // Generate QR code
            string qrContent = $"PAY|OrderID={order.OrderId}|Amount={amount:F2}|Time={DateTime.Now:yyyyMMddHHmmss}";

            // Show QR payment dialog
            using (var qrDialog = new Form())
            {
                qrDialog.Text = "QR Code Payment";
                qrDialog.Width = 500;
                qrDialog.Height = 600;
                qrDialog.StartPosition = FormStartPosition.CenterParent;
                qrDialog.FormBorderStyle = FormBorderStyle.FixedDialog;

                // Amount label
                var lblAmount = new Label
                {
                    Text = $"Amount to Pay: ${amount:F2}",
                    Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                    AutoSize = true,
                    Location = new System.Drawing.Point(20, 20)
                };

                // Instruction label
                var lblInstruction = new Label
                {
                    Text = "Scan QR code with your mobile banking app",
                    AutoSize = true,
                    Location = new System.Drawing.Point(20, 60)
                };

                // QR Code placeholder (you need QRCoder NuGet package)
                var pbQR = new PictureBox
                {
                    Width = 400,
                    Height = 400,
                    Location = new System.Drawing.Point(50, 90),
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.CenterImage
                };

                // Generate QR code image
                try
                {
                    using var qrGenerator = new QRCoder.QRCodeGenerator();
                    using var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);
                    using var qrCode = new QRCoder.QRCode(qrCodeData);
                    pbQR.Image = qrCode.GetGraphic(20);
                }
                catch
                {
                    // Fallback if QRCoder fails
                    pbQR.BackColor = System.Drawing.Color.LightGray;
                    var lblQRText = new Label
                    {
                        Text = "QR Code\n" + qrContent,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Font = new System.Drawing.Font("Segoe UI", 9)
                    };
                    pbQR.Controls.Add(lblQRText);
                }

                // Confirm button
                var btnConfirm = new Button
                {
                    Text = "I've Paid - Confirm",
                    Width = 200,
                    Height = 45,
                    Location = new System.Drawing.Point(50, 500),
                    BackColor = System.Drawing.Color.FromArgb(0, 123, 255),
                    ForeColor = System.Drawing.Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };

                btnConfirm.Click += (s, e) =>
                {
                    // Simulate payment verification
                    _transactionId = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();

                    MessageBox.Show(
                        $"QR Payment Verified!\n\nTransaction ID: {_transactionId}\nAmount: ${amount:F2}",
                        "Payment Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    qrDialog.DialogResult = DialogResult.OK;
                };

                // Cancel button
                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Width = 150,
                    Height = 45,
                    Location = new System.Drawing.Point(260, 500),
                    DialogResult = DialogResult.Cancel
                };

                qrDialog.Controls.AddRange(new Control[] {
                    lblAmount, lblInstruction, pbQR, btnConfirm, btnCancel
                });

                return qrDialog.ShowDialog() == DialogResult.OK;
            }
        }

        public string GetPaymentMethodName() => "QR";

        public void PrintReceipt(Order order)
        {
            string receipt = $@"
========================================
           RESTAURANT POS RECEIPT
========================================
Order ID:        {order.OrderId}
Date:            {DateTime.Now:yyyy-MM-dd HH:mm:ss}
Table:           {order.TableId?.ToString() ?? "Takeaway"}
----------------------------------------
Total Amount:    ${order.TotalAmount:F2}
Payment Method:  QR Code
Transaction ID:  {_transactionId}
========================================
        Thank you for your business!
========================================
";
            Console.WriteLine(receipt);
            MessageBox.Show(receipt, "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool CanProcess(decimal amount) => amount > 0;
    }

    #endregion

    #region Payment Context

    /// <summary>
    /// Context class that uses a payment strategy
    /// </summary>
    public class PaymentContext
    {
        private IPaymentStrategy? _strategy;

        /// <summary>
        /// Set the payment strategy at runtime
        /// </summary>
        public void SetStrategy(IPaymentStrategy strategy)
        {
            _strategy = strategy;
            Console.WriteLine($"[PaymentContext] Strategy set to: {strategy.GetPaymentMethodName()}");
        }

        /// <summary>
        /// Execute payment using the selected strategy
        /// </summary>
        public bool ExecutePayment(decimal amount, Order order)
        {
            if (_strategy == null)
            {
                MessageBox.Show("Please select a payment method first!",
                    "No Payment Method",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (!_strategy.CanProcess(amount))
            {
                MessageBox.Show("Invalid payment amount!",
                    "Payment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            bool success = _strategy.ProcessPayment(amount, order);

            if (success)
            {
                _strategy.PrintReceipt(order);
            }

            return success;
        }

        /// <summary>
        /// Get current payment method name
        /// </summary>
        public string GetCurrentMethod()
        {
            return _strategy?.GetPaymentMethodName() ?? "None";
        }
    }

    #endregion
}