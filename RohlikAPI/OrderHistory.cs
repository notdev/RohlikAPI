﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RohlikAPI.Helpers;
using RohlikAPI.Model;
using RohlikAPI.Model.JsonDeserialization;

namespace RohlikAPI
{
    internal class OrderHistory
    {
        private const string BaseUrl = "https://www.rohlik.cz/uzivatel/profil";

        private readonly PersistentSessionHttpClient httpClient;
        private readonly PriceParser priceParser = new PriceParser();

        internal OrderHistory(PersistentSessionHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal IEnumerable<Order> GetOrdersSinceDate(DateTime getOrdersSince)
        {
            var page = 0;

            var ordersToReturn = new List<Order>();
            var alreadySeenOrders = new HashSet<int>();

            var stopGettingOrders = false;

            while (!stopGettingOrders)
            {
                page++;
                var ordersThisPage = GetOrdersForPage(page);
                foreach (var order in ordersThisPage)
                {
                    if (alreadySeenOrders.Contains(order.Id) || order.Date < getOrdersSince)
                    {
                        stopGettingOrders = true;
                        break;
                    }

                    ordersToReturn.Add(order);
                    alreadySeenOrders.Add(order.Id);
                }
            }

            return ordersToReturn;
        }

        private IEnumerable<Order> GetOrdersForPage(int page)
        {
            var order = GetOrderHistoryForPage(page, BaseUrl);
            var parsedOrders = ParseOrderHistory(order);
            return parsedOrders;
        }

        private IEnumerable<Order> ParseOrderHistory(HtmlDocument document)
        {
            var orderNodes = document.DocumentNode.SelectNodes("//ul");
           
            var returnList = orderNodes.Select(ParseOrderNode);
            return returnList;
        }

        private Order ParseOrderNode(HtmlNode orderNode)
        {
            var orderIdString = orderNode.SelectSingleNode(".//li/strong").InnerText;

            var orderId = int.Parse(orderIdString.Replace("#", ""));

            var dateString = orderNode.SelectSingleNode(".//li[@class='date']/span").InnerText;

            var orderDate = DateTime.Parse(dateString.Replace(" ", ""), new CultureInfo("cs-CZ"));

            var priceString = orderNode.SelectSingleNode(".//li[@class='price']/span").InnerText;

            var price = double.Parse(priceString.Replace("Kč", "").Replace("&nbsp;", "").Trim());

            var paymentMethod = orderNode.SelectSingleNode(".//li[@class='payment']/span").InnerText;
            
            var articlesNode = orderNode.SelectSingleNode($"//div[@class='items o{orderId}']");
            var articles = ParseOrderArticles(articlesNode);

            var order = new Order
            {
                Id = orderId,
                Price = price,
                Date = orderDate,
                PaymentMethod = paymentMethod,
                Articles = articles
                
            };
            return order;
        }

        private IEnumerable<Article> ParseOrderArticles(HtmlNode orderNode)
        {
            var articleNodes = orderNode.SelectNodes(".//article");
            return articleNodes.Select(ParseSingleArticle);
        }

        private Article ParseSingleArticle(HtmlNode articleNode)
        {
            var articleNameNode = articleNode.SelectSingleNode(".//h3/a") ?? articleNode.SelectSingleNode(".//h3");
            string articleName = articleNameNode.InnerText.Trim();

            var articleCountString = articleNode.SelectSingleNode(".//p").InnerText;
            
            var cleanArticleCountString = Regex.Match(articleCountString, @"\d*").Value;
            int articleCount;
            int.TryParse(cleanArticleCountString, out articleCount);
            
            var priceNode = articleNode.SelectSingleNode(".//h6/strong");
            double price = priceParser.ParsePrice(priceNode.InnerText);

            return new Article
            {
                Count = articleCount,
                Name = articleName,
                Price = price
            };
        }

        private HtmlDocument GetOrderHistoryForPage(int page, string baseUrl)
        {
            if (page < 1)
            {
                throw new ArgumentException("Page must be greater than 0");
            }

            string command;
            if (page == 1)
            {
                command = "ordersHistoryGrocery-prevPage";
            }
            else
            {
                page--;
                command = "ordersHistoryGrocery-nextPage";
            }
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}?ordersHistoryGrocery-page={page}&do={command}");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var stringResponse = httpClient.Get(request);
            var result = JsonConvert.DeserializeObject<OrdersResponse>(stringResponse);

            var snipets = result.OrderSnippets;
            if (snipets?.SnippetOrdersHistoryGrocery == null)
            {
                throw new Exception($"Failed to get Order history for page {page}");
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(snipets.SnippetOrdersHistoryGrocery);

            return htmlDocument;
        }
    }
}