using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GetPassbookModel : MonoBehaviour
{
    public int id_CouponsRedeemed { get; set; }
    public int? CouponID { get; set; }
    public string WebsiteName { get; set; }
    public string CouponCode { get; set; }
    public string CouponTitle { get; set; }
    public string CouponDescription { get; set; }
    public string Link { get; set; }
    public int? Debit_Coins { get; set; }
    public string Image { get; set; }
    public string ExpiryDate { get; set; }
    public string status { get; set; }
    public string UPDATED_DATE_TIME { get; set; }
    public DateTime UpdatedDate
    {
        get
        {
            var date = UPDATED_DATE_TIME.Split(" "[0]);
            return Convert.ToDateTime(date[0], CultureInfo.InvariantCulture);
        }
    }
        
    public int? id_rewards { get; set; }
    public int Card_type { get; set; }
    public int id_log_scratchcard { get; set; }
    public int Id_User { get; set; }
    public int id_scratchcard { get; set; }
    public string scratchcard { get; set; }
    public int? scratch_point { get; set; }
    public int? Credit_Coins { get; set; }
    public int? ID_ORGANIZATION { get; set; }
    public int? IsScratch { get; set; }
    public DateTime shortingdate { get; set; }
}

