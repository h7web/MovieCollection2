//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace herman_v2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class image
    {
        public int image_id { get; set; }
        public string image_file { get; set; }
        public Nullable<int> image_type { get; set; }
        public int video_id { get; set; }
    
        public virtual Video Video { get; set; }
    }
}
