using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Reflection;
using Site.App_Code;

namespace Site.Handlers.Chart
{
    /// <summary>
    /// Summary description for ChartData
    /// </summary>
    public class ChartData : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                if (!String.IsNullOrWhiteSpace(context.Request.Form["t"])) // && !String.IsNullOrWhiteSpace(context.Request.QueryString["bf"])
                {
                    switch (context.Request.Form["t"].ToLower())
                    {
                        case "score_per_block":
                            {
                                long responseFormId = 0;
                                long.TryParse(context.Request.Form["rfid"], out responseFormId);

                                context.Response.Write(getBlockScoreChart(responseFormId));
                                context.Response.StatusCode = 200;

                                break;
                            }
                        case "score_per_form":
                            {
                                long responseFormId = 0;
                                long.TryParse(context.Request.Form["rfid"], out responseFormId);

                                context.Response.Write(getScoreFormChart(responseFormId));
                                context.Response.StatusCode = 200;

                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                context.Response.StatusCode = 500;
            }
        }

        private string getBlockScoreChart(long responseFormId)
        {
            Lib.Report.ReportManager manager = new Lib.Report.ReportManager(this.ActiveUser);
            List<Lib.Report.ScorePerBlock> report = manager.generateScorePerBlockChart(responseFormId);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(report);
        }

        private string getScoreFormChart(long responseFormId)
        {
            Lib.Report.ReportManager manager = new Lib.Report.ReportManager(this.ActiveUser);
            List<Lib.Report.ScorePerBlock> report = manager.generateScorePerFormChart(responseFormId);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(report);
        }
    }
}