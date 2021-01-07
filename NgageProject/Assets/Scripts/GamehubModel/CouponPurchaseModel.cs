using System;

public class CouponPurchaseModel 
{
    public int id_CouponsRedeemed { get; set; }
    public string CouponID { get; set; }
    public string WebsiteName { get; set; }
    public string CouponCode { get; set; }
    public string CouponTitle { get; set; }
    public string CouponDescription { get; set; }
    public string Link { get; set; }
    public string PointsUsed { get; set; }
    public string Image { get; set; }
    public string ExpiryDate { get; set; }
    public int id_user { get; set; }
    public object id_organization { get; set; }
    public string status { get; set; }
    public DateTime UPDATED_DATE_TIME { get; set; }
    public int id_rewards { get; set; }
    public int Card_type { get; set; }
}
