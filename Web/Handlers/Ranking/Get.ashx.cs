using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Site.Handlers.Ranking
{
    /// <summary>
    /// Summary description for Get
    /// </summary>
    public class Get : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                using (Lib.Repositories.ResponseFormRepository rep = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                {
                    var responseForms = rep.getRanking();

                    var track = new List<string>();

                    var result = responseForms.Select(rf => new
                    {
                        Id = rf.Id,
                        TotalScore = String.Format("{0}", Math.Round(rf.TotalScore,2)),
                        CityState = String.Format("{0}/{1}", rf.City.Name, rf.City.StateId),
                        State = rf.City.StateId,
                        BaseBlocks = rf.BaseForm.BaseBlocks.Select(bb => new
                        {
                            Id = bb.Id,
                            IdCrypt = Commons.SecurityUtils.criptografar(bb.Id.ToString()),
                            Name = bb.Name,
                            Score = bb.calculateNB(rf.Id, ref track),
                            BaseSubblocks = bb.BaseSubBlocks.Select(bsb => new
                            {
                                Id = bsb.Id,
                                Letter = bsb.Name[0].ToString().ToUpper(),
                                Name = bsb.Name,
                                Graphic = String.Format("{0}/{1}. <br/> {2} N/A.", "2", "5", "15"),
                                Percent = bsb.calculatePercent(rf.Id),
                                Color = bsb.getColorByPercent(rf.Id)
                            })
                        })
                    }).ToList();

                    context.Response.Write(serializer.Serialize(result));
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Site.Handlers.Ranking.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
        }
    }
}