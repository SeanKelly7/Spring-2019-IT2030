using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EventApplication.Models
{
    public class EventType : IValidatableObject
    {
        [Display(Name = "Event Type ID")]
        public virtual int EventTypeId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Type")]
        public virtual string Title { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.Length > 50)
            {
                yield return new ValidationResult("Type should not exceed 50 characters", new[] { "Title" });
            }
        }
    }
}