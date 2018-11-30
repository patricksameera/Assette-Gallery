using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Assette.Web.ReportsGallery
{
    public partial class refresh : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearApplicationCache();

            Response.Redirect("client-welcome.aspx");
        }

        public void ClearApplicationCache()
        {
            /*
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator 
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();

            // copy all keys that currently exist in Cache 
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache 
            for (int i = 0; i < keys.Count; i++)
            {
                Cache.Remove(keys[i]);
            }*/

            /*
            foreach (DictionaryEntry item in Cache)
            {
                Cache.Remove(item.Key.ToString());
                Response.Write(item.Key.ToString() + " removed<br />");
            }*/

            //HttpResponse.RemoveOutputCacheItem("../template-designs.aspx");

            HttpResponse.RemoveOutputCacheItem("/caching/template-designs.aspx");
        }
    }
}