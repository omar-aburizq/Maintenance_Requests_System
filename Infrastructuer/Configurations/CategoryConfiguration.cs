using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructuer.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData
            (

                new Category
                {
                    Id = Guid.Parse("e8097122-bd95-4b96-bbe7-292241e044b8"),
                    Name = "Electrical",
                    Description = "All issues related to electrical systems, including power outages, wiring faults, lighting problems, and electrical equipment failures."
                },
                new Category
                {
                    Id = Guid.Parse("f21254f2-7cc3-4cea-886e-bcded6cf9492"),
                    Name = "InformationTechnology",
                    Description = "Issues related to computers, software, systems, and technical support such as system errors, software installation, and device troubleshooting."
                },
                new Category
                {
                    Id = Guid.Parse("7c20c266-0969-431b-b0a3-bb1a65d4baa5"),
                    Name = "Networking",
                    Description = "Problems related to internet connectivity, network devices, Wi-Fi issues, and communication systems."
                },
                new Category
                {
                    Id = Guid.Parse("af51617f-64c4-409f-8867-749ca8a5ef83"),
                    Name = "Plumbing",
                    Description = "Problems related to water systems including leaks, pipe blockages, drainage issues, and maintenance of plumbing fixtures."
                },
                new Category
                {
                    Id = Guid.Parse("d8ed6dd0-83c5-4d78-93e9-58a138b8ccd8"),
                    Name = "HVAC",
                    Description = "Issues related to heating, ventilation, and air conditioning systems including cooling/heating failures and maintenance."
                }
            );
        }
    }
}
