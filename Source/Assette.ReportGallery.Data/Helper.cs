using System;
using System.Data;
using System.Text;

namespace Assette.ReportGallery.Data
{
    public class Helper
    {
        public string JsonForJqgrid(DataTable dt, int pageSize, int totalRecords, int page)
        {
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            StringBuilder jsonBuilder = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Append("{");
                jsonBuilder.Append("\"total\":" + totalPages + ",\"page\":" + page + ",\"records\":" + (totalRecords) + ",\"rows\"");
                jsonBuilder.Append(":[");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{\"i\":" + (i) + ",\"cell\":[");

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");

                        //jsonBuilder.Append(dt.Rows[i][j].ToString());

                        // replace double quote with single quote
                        jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "'"));

                        jsonBuilder.Append("\",");
                    }

                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("]},");
                }

                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]");
                jsonBuilder.Append("}");
            }
            else
            {
                jsonBuilder.Append("{\"total\":0,\"page\":0,\"records\":0,\"rows\":[]}");
            }

            // sample
            //{"total":5,"page":1,"records":47,"rows":[{"i":0,"cell":["9db67f26-92f7-4e90-93a9-5e0317265068","Sameera","Jayalath","sameera.jayalath@assette.com","Assette","192.168.0.150","11/2/2012 8:57:38 AM","0"]},{"i":1,"cell":["d371ba9d-b5bb-4229-bd51-a48f8cb3449f","...
            //{"total":5,"page":1,"records":47,"rows":[]}

            return jsonBuilder.ToString();
        }
    }
}
