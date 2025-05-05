using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountReadModels;
using TMQ.BaseDomains;
using TMQ.EnumDefine;

namespace TMQ.AccountDomains.Entities
{
    [Table("Address_tbl")]
    public class Address : BaseDomain
    {
        public Address(RAddress address) : base(address) 
        {

        }
        public new string? Id { get; set; }
        public StatusEnum Status { get; set; }
        public string? UserId { get; set; }
        public string? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public int? StreetId { get; set; }
        public string? Detail { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public AddressTypeEnum Type { get; set; }
    }
}
