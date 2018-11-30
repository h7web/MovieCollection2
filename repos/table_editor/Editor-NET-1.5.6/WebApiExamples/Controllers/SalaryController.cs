using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using DataTables;
using WebApiExamples.Models;


namespace WebApiExamples.Controllers
{
    /// <summary>
    /// This is controller is used by the majority of Editor examples as it
    /// provides a nice rounded set of information for the client-side Editor
    /// Javascript library to show its capabilities.
    ///
    /// In the code here, note that the `StaffModel` is used as the model for
    /// the Editor, which automatically defines the database fields to be read.
    /// Additional instructions can be given for each field by creating a `Field`
    /// instance for it - many of the fields have validation methods applied here
    /// and the date field has a formatter to make it readable to users looking
    /// at the table!
    /// </summary>
    public class SalaryController : ApiController
    {
        [Route("api/salary/ap")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult Salary()
        {
            var request = HttpContext.Current.Request;
            var settings = Properties.Settings.Default;

            using (var db = new Database(settings.DbType, settings.DbConnection2))
            {
                var response = new Editor(db, "employees")
                    .Model<SalaryModel>()
                    .TryCatch(false)
                    .Field(new Field("employees.fullname"))
                    .Field(new Field("employees.univ_start_date").Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.library_start_date").Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.uin")
                    .Validator(Validation.Numeric())
                    )
                   .Field(new Field("employees.birth_dt")
                   .Validator(Validation.DateFormat(
                   Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )

                    .LeftJoin("positions", "positions.EDW_PERS_ID", "=", "employees.id")
                    .Where("positions.empee_cls_cd", "BA")
                    .Where("positions.job_detl_fte", "1")
                    .Field(new Field("positions.job_detl_title"))
                    .Field(new Field("positions.job_detl_sub_dept_level_6_name"))
                    .Field(new Field("positions.job_detl_annl_sal"))
                    .Field(new Field("positions.empee_cls_cd"))
                    .Field(new Field("positions.org_cd"))

                    .LeftJoin("SWS_entries", "SWS_entries.EDW_PERS_ID", "=", "employees.id")

                    .Field(new Field("SWS_entries.fy"))
                    .Field(new Field("SWS_entries.frcscore"))
                    .Field(new Field("SWS_entries.frcvariable"))
                    .Field(new Field("SWS_entries.totalmerit"))
                    .Field(new Field("SWS_entries.meritincrease"))
                    .Field(new Field("SWS_entries.equityadjust"))
                    .Field(new Field("SWS_entries.p_t"))
                    .Field(new Field("SWS_entries.evalscore"))
                    .Field(new Field("SWS_entries.stipend"))
                    .Field(new Field("SWS_entries.other"))
                    .Field(new Field("SWS_entries.notes"))



                    .Process(request)
                    .Data();

                return Json(response);
            }
        }
    }
    public class SalaryALController : ApiController
    {
        [Route("api/salary/al")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult Salary()
        {
            var request = HttpContext.Current.Request;
            var settings = Properties.Settings.Default;

            using (var db = new Database(settings.DbType, settings.DbConnection2))
            {
                var response = new Editor(db, "employees")
                    .Model<SalaryModel>()
                    .TryCatch(false)
                    .Field(new Field("employees.fullname"))
                    .Field(new Field("employees.univ_start_date").Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.library_start_date").Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.uin")
                    .Validator(Validation.Numeric())
                    )
                   .Field(new Field("employees.birth_dt")
                   .Validator(Validation.DateFormat(
                   Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )

                    .LeftJoin("positions", "positions.EDW_PERS_ID", "=", "employees.id")
                    .Where("positions.empee_cls_cd", "AL")
                    .Where("positions.job_detl_fte", "1")
                    .Field(new Field("positions.job_detl_title"))
                    .Field(new Field("positions.job_detl_sub_dept_level_6_name"))
                    .Field(new Field("positions.job_detl_annl_sal"))
                    .Field(new Field("positions.empee_cls_cd"))
                    .Field(new Field("positions.org_cd"))
                    .Field(new Field("positions.fac_rank_desc"))
                    .Field(new Field("positions.adrole"))

                    .LeftJoin("SWS_entries", "SWS_entries.EDW_PERS_ID", "=", "employees.id")

                    .Field(new Field("SWS_entries.fy"))
                    .Field(new Field("SWS_entries.frcscore"))
                    .Field(new Field("SWS_entries.frcvariable"))
                    .Field(new Field("SWS_entries.totalmerit"))
                    .Field(new Field("SWS_entries.meritincrease"))
                    .Field(new Field("SWS_entries.equityadjust"))
                    .Field(new Field("SWS_entries.p_t"))
                    .Field(new Field("SWS_entries.evalscore"))
                    .Field(new Field("SWS_entries.stipend"))
                    .Field(new Field("SWS_entries.other"))
                    .Field(new Field("SWS_entries.notes"))



                    .Process(request)
                    .Data();

                return Json(response);
            }
        }
    }

    public class SalaryCSController : ApiController
    {
        [Route("api/salary/cs")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult Salary()
        {
            var request = HttpContext.Current.Request;
            var settings = Properties.Settings.Default;

            using (var db = new Database(settings.DbType, settings.DbConnection2))
            {
                var response = new Editor(db, "employees")
                    .Model<SalaryModel>()
                    .TryCatch(false)
                    .Field(new Field("employees.fullname"))
                    .Field(new Field("employees.univ_start_date").Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.library_start_date").Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.uin")
                    .Validator(Validation.Numeric())
                    )
                   .Field(new Field("employees.birth_dt")
                   .Validator(Validation.DateFormat(
                   Format.DATE_ISO_8601
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("employees.netid"))


                    .LeftJoin("positions", "positions.EDW_PERS_ID", "=", "employees.id")
                    .Where( q =>
                        q.Where("positions.empee_cls_cd", "(SELECT empee_cls_cd FROM positions WHERE empee_cls_cd like 'C%')", "IN", false)
                        )
                    .Field(new Field("positions.job_detl_title"))
                    .Field(new Field("positions.job_detl_sub_dept_level_6_name"))
                    .Field(new Field("positions.job_detl_annl_sal"))
                    .Field(new Field("positions.empee_cls_cd"))
                    .Field(new Field("positions.posn_nbr"))
                    .Field(new Field("positions.job_detl_fte"))
                    .Field(new Field("positions.job_probn_bgn_dt"))
                    .Field(new Field("positions.job_probn_end_dt"))
                    .Field(new Field("positions.posn_exempt_ind"))

                    .LeftJoin("SWS_entries", "SWS_entries.EDW_PERS_ID", "=", "employees.id")

                    .Field(new Field("SWS_entries.fy"))
                    .Field(new Field("SWS_entries.frcscore"))
                    .Field(new Field("SWS_entries.frcvariable"))
                    .Field(new Field("SWS_entries.totalmerit"))
                    .Field(new Field("SWS_entries.meritincrease"))
                    .Field(new Field("SWS_entries.equityadjust"))
                    .Field(new Field("SWS_entries.p_t"))
                    .Field(new Field("SWS_entries.evalscore"))
                    .Field(new Field("SWS_entries.stipend"))
                    .Field(new Field("SWS_entries.other"))
                    .Field(new Field("SWS_entries.notes"))



                    .Process(request)
                    .Data();

                return Json(response);
            }
        }
    }
}
