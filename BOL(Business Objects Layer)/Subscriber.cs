using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class Subscriber
    {
        public string SubscriberId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string WhatsAppNo { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Education { get; set; }
        public string JobProfile { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Marital { get; set; }
        public string MemberTypeId { get; set; }
        public string MemberType { get; set; }
        public string IsForwarder { get; set; }
        public string ParentId { get; set; }
        public string IsDeActive { get; set; }
        public string Discount { get; set; }

        //new field added
        public string partytype { get; set; }
        public string gstin { get; set; }
        public string panno { get; set; }
        public string aadharno { get; set; }
        public string contactperson { get; set; }

        public List<Address> address { get; set; }
    }
    public class Address 
    {
        public string SubscriberId { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public string LandMark { get; set; }
        public string LocalAddress { get; set; }
        public string Status { get; set; }
    }
}
