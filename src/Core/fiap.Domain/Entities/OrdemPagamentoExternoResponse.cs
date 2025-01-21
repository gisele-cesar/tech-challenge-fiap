using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Domain.Entities
{
    public class OrdemPagamentoExternoResponse
    {
        public long id { get; set; }
        public string status { get; set; }
        public string external_reference { get; set; }
        public string preference_id { get; set; }
        public List<object> payments { get; set; }
        public List<object> shipments { get; set; }
        public List<object> payouts { get; set; }
        public Collector collector { get; set; }
        public string marketplace { get; set; }
        public string notification_url { get; set; }
        public DateTime date_created { get; set; }
        public DateTime last_updated { get; set; }
        public object sponsor_id { get; set; }
        public decimal shipping_cost { get; set; }
        public decimal total_amount { get; set; }
        public string site_id { get; set; }
        public decimal paid_amount { get; set; }
        public decimal refunded_amount { get; set; }
        public object payer { get; set; }
        public List<ItemResponse> items { get; set; }
        public bool cancelled { get; set; }
        public string additional_info { get; set; }
        public object application_id { get; set; }
        public bool is_test { get; set; }
        public string order_status { get; set; }
        public string client_id { get; set; }
    }

    public class Collector
    {
        public long id { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
    }

    public class ItemResponse
    {
        public string id { get; set; }
        public string category_id { get; set; }
        public string currency_id { get; set; }
        public string description { get; set; }
        public string picture_url { get; set; }
        public string title { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
    }
}
