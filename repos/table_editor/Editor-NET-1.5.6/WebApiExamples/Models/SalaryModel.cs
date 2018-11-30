using System.ComponentModel;
using DataTables;

namespace WebApiExamples.Models
{
    public class SalaryModel : EditorModel
    {
        public class employees
        {
            public string fullname { get; set; }

            public string yr_of_svc { get; set; }

            public string univ_start_date { get; set; }

            public string library_start_date { get; set; }

            public string uin { get; set; }

            public string birth_dt { get; set; }

        }

        public class positions : EditorModel
        {
            public string job_detl_title { get; set; }

            public string job_detl_sub_dept_level_6_name { get; set; }

            public float job_detl_annl_sal { get; set; }

            public string empee_cls_cd { get; set; }

            public string org_cd { get; set; }

            public string fac_rank_desc { get; set; }

            public string fac_tenure_track_yr { get; set; }

            public string adrole { get; set; }

        }

        public class SWS_entries : EditorModel 
        {
            public string fy { get; set; }

            public string frcscore { get; set; }

            public string frcvariable { get; set; }

            public float totalmerit { get; set; }

            public float meritincrease { get; set; }

            public float equityadjust { get; set; }

            public float p_t { get; set; }

            public float evalscore { get; set; }

            public float stipend { get; set; }

            public float other { get; set; }

            public string notes { get; set; }
        }
    }
}