using System;

namespace OOAD_Project.Patterns
{
    /// <summary>
    /// DECORATOR PATTERN IMPLEMENTATION
    /// Allows adding new functionality to products dynamically without modifying their structure.
    /// Useful for add-ons, discounts, taxes, special offers, etc.
    /// </summary>

    #region Product Interface

    /// <summary>
    /// Base product interface that all products and decorators implement
    /// </summary>
    public interface IProduct
    {
        int GetProductId();
        string GetName();
        string GetDescription();
        decimal GetPrice();
        string GetCategory();
    }

    #endregion

    #region Base Product Implementation

    /// <summary>
    /// Concrete product class - the base component we'll decorate
    /// </summary>
    public class BaseProduct : IProduct
    {
        protected int _productId;
        protected string _name;
        protected string _baseDescription;
        protected decimal _basePrice;
        protected string _category;

        public BaseProduct(int productId, string name, decimal price, string category, string description = "")
        {
            _productId = productId;
            _name = name;
            _basePrice = price;
            _category = category;
            _baseDescription = string.IsNullOrEmpty(description) ? name : description;
        }

        public virtual int GetProductId() => _productId;
        public virtual string GetName() => _name;
        public virtual string GetDescription() => _baseDescription;
        public virtual decimal GetPrice() => _basePrice;
        public virtual string GetCategory() => _category;

        public override string ToString()
        {
            return $"{GetName()} - ${GetPrice():F2}";
        }
    }

    #endregion

    #region Abstract Decorator

    /// <summary>
    /// Abstract decorator class - base for all concrete decorators
    /// </summary>
    public abstract class ProductDecorator : IProduct
    {
        protected IProduct _product;

        protected ProductDecorator(IProduct product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
        }

        public virtual int GetProductId() => _product.GetProductId();
        public virtual string GetName() => _product.GetName();
        public virtual string GetDescription() => _product.GetDescription();
        public virtual decimal GetPrice() => _product.GetPrice();
        public virtual string GetCategory() => _product.GetCategory();
    }

    #endregion

    #region Concrete Decorators

    /// <summary>
    /// Extra Cheese Decorator - adds cheese topping
    /// </summary>
    public class ExtraCheeseDecorator : ProductDecorator
    {
        private const decimal EXTRA_CHEESE_COST = 1.50m;

        public ExtraCheeseDecorator(IProduct product) : base(product) { }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} + Extra Cheese";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() + EXTRA_CHEESE_COST;
        }
    }

    /// <summary>
    /// Extra Toppings Decorator - generic topping
    /// </summary>
    public class ExtraToppingDecorator : ProductDecorator
    {
        private readonly string _toppingName;
        private readonly decimal _toppingCost;

        public ExtraToppingDecorator(IProduct product, string toppingName, decimal cost)
            : base(product)
        {
            _toppingName = toppingName;
            _toppingCost = cost;
        }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} + {_toppingName}";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() + _toppingCost;
        }
    }

    /// <summary>
    /// Large Size Decorator - upsize product
    /// </summary>
    public class LargeSizeDecorator : ProductDecorator
    {
        private const decimal SIZE_UPCHARGE = 2.00m;

        public LargeSizeDecorator(IProduct product) : base(product) { }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} (Large)";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() + SIZE_UPCHARGE;
        }
    }

    /// <summary>
    /// Extra Large Size Decorator
    /// </summary>
    public class ExtraLargeSizeDecorator : ProductDecorator
    {
        private const decimal XL_UPCHARGE = 3.50m;

        public ExtraLargeSizeDecorator(IProduct product) : base(product) { }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} (Extra Large)";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() + XL_UPCHARGE;
        }
    }

    /// <summary>
    /// Discount Decorator - apply percentage discount
    /// </summary>
    public class DiscountDecorator : ProductDecorator
    {
        private readonly decimal _discountPercentage;
        private readonly string _discountReason;

        public DiscountDecorator(IProduct product, decimal discountPercentage, string reason)
            : base(product)
        {
            _discountPercentage = Math.Max(0, Math.Min(100, discountPercentage)); // 0-100%
            _discountReason = reason;
        }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} ({_discountPercentage}% OFF - {_discountReason})";
        }

        public override decimal GetPrice()
        {
            decimal originalPrice = _product.GetPrice();
            decimal discountAmount = originalPrice * (_discountPercentage / 100m);
            return originalPrice - discountAmount;
        }
    }

    /// <summary>
    /// Tax Decorator - add tax to price
    /// </summary>
    public class TaxDecorator : ProductDecorator
    {
        private readonly decimal _taxRate;

        public TaxDecorator(IProduct product, decimal taxRate = 0.10m) : base(product)
        {
            _taxRate = taxRate; // Default 10%
        }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} (incl. {_taxRate * 100}% tax)";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() * (1 + _taxRate);
        }
    }

    /// <summary>
    /// Combo Deal Decorator - bundle discount
    /// </summary>
    public class ComboDecorator : ProductDecorator
    {
        private readonly decimal _comboDiscount;

        public ComboDecorator(IProduct product, decimal comboDiscount = 2.00m) : base(product)
        {
            _comboDiscount = comboDiscount;
        }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} (Combo Deal - Save ${_comboDiscount:F2})";
        }

        public override decimal GetPrice()
        {
            return Math.Max(0, _product.GetPrice() - _comboDiscount);
        }
    }

    /// <summary>
    /// Rush Order Decorator - premium for urgent orders
    /// </summary>
    public class RushOrderDecorator : ProductDecorator
    {
        private const decimal RUSH_FEE = 3.00m;

        public RushOrderDecorator(IProduct product) : base(product) { }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} (Rush Order)";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() + RUSH_FEE;
        }
    }

    /// <summary>
    /// Gift Wrap Decorator - add gift packaging
    /// </summary>
    public class GiftWrapDecorator : ProductDecorator
    {
        private const decimal GIFT_WRAP_COST = 2.50m;

        public GiftWrapDecorator(IProduct product) : base(product) { }

        public override string GetDescription()
        {
            return $"{_product.GetDescription()} (Gift Wrapped)";
        }

        public override decimal GetPrice()
        {
            return _product.GetPrice() + GIFT_WRAP_COST;
        }
    }

    #endregion

    #region Helper/Builder Class

    /// <summary>
    /// Product builder to make it easier to apply multiple decorators
    /// </summary>
    public class ProductBuilder
    {
        private IProduct _product;

        public ProductBuilder(IProduct baseProduct)
        {
            _product = baseProduct;
        }

        public ProductBuilder AddExtraCheese()
        {
            _product = new ExtraCheeseDecorator(_product);
            return this;
        }

        public ProductBuilder AddTopping(string name, decimal cost)
        {
            _product = new ExtraToppingDecorator(_product, name, cost);
            return this;
        }

        public ProductBuilder MakeLarge()
        {
            _product = new LargeSizeDecorator(_product);
            return this;
        }

        public ProductBuilder MakeExtraLarge()
        {
            _product = new ExtraLargeSizeDecorator(_product);
            return this;
        }

        public ProductBuilder ApplyDiscount(decimal percentage, string reason)
        {
            _product = new DiscountDecorator(_product, percentage, reason);
            return this;
        }

        public ProductBuilder AddTax(decimal taxRate = 0.10m)
        {
            _product = new TaxDecorator(_product, taxRate);
            return this;
        }

        public ProductBuilder MakeCombo(decimal discount = 2.00m)
        {
            _product = new ComboDecorator(_product, discount);
            return this;
        }

        public ProductBuilder MakeRush()
        {
            _product = new RushOrderDecorator(_product);
            return this;
        }

        public ProductBuilder AddGiftWrap()
        {
            _product = new GiftWrapDecorator(_product);
            return this;
        }

        public IProduct Build()
        {
            return _product;
        }
    }

    #endregion
}