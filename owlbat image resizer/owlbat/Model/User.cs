namespace owlbat.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(60)]
        public string UserMail { get; set; }

        public bool Sex { get; set; }

        [StringLength(250)]
        public string PostDescription { get; set; }

        [StringLength(60)]
        public string PostPrecondition { get; set; }

        [StringLength(3)]
        public string AccountType { get; set; }

        public byte Rating { get; set; }

        [StringLength(60)]
        public string PostPostcondition { get; set; }
    }
}
