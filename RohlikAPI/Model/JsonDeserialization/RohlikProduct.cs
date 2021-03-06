﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class RohlikProduct
    {
        [JsonProperty("productId")]
        public long ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("mainCategoryId")]
        public long MainCategoryId { get; set; }

        [JsonProperty("imgPath")]
        public string ImgPath { get; set; }

        [JsonProperty("baseLink")]
        public string BaseLink { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("expectedAvailability")]
        public long? ExpectedAvailability { get; set; }

        [JsonProperty("unavailabilityReason")]
        public object UnavailabilityReason { get; set; }

        [JsonProperty("preorderEnabled")]
        public bool PreorderEnabled { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("textualAmount")]
        public string TextualAmount { get; set; }

        [JsonProperty("semicaliber")]
        public bool Semicaliber { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("pricePerUnit")]
        public Price PricePerUnit { get; set; }

        [JsonProperty("recommendedPricePerUnit")]
        public Price RecommendedPricePerUnit { get; set; }

        [JsonProperty("originalPrice")]
        public OriginalPrice OriginalPrice { get; set; }

        [JsonProperty("goodPrice")]
        public bool GoodPrice { get; set; }

        [JsonProperty("goodPriceSalePercentage")]
        public long GoodPriceSalePercentage { get; set; }

        [JsonProperty("sales")]
        public List<Sale> Sales { get; set; }

        [JsonProperty("maxBasketAmount")]
        public long MaxBasketAmount { get; set; }

        [JsonProperty("maxBasketAmountReason")]
        public string MaxBasketAmountReason { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("badge")]
        public List<Badge> Badge { get; set; }

        [JsonProperty("stars")]
        public object Stars { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("countries")]
        public List<Country> Countries { get; set; }

        [JsonProperty("imageScaleRatio")]
        public long ImageScaleRatio { get; set; }

        [JsonProperty("imagesCount")]
        public long ImagesCount { get; set; }

        [JsonProperty("immediateConsumption")]
        public bool ImmediateConsumption { get; set; }

        [JsonProperty("watchDog")]
        public bool WatchDog { get; set; }

        [JsonProperty("composition")]
        public Composition Composition { get; set; }

        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("productStory")]
        public ProductStory ProductStory { get; set; }

        [JsonProperty("vivino")]
        public object Vivino { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("inStockByAmount")]
        public bool InStockByAmount { get; set; }

        [JsonProperty("inStock")]
        public bool InStock { get; set; }

        [JsonProperty("favourite")]
        public bool Favourite { get; set; }
    }
}