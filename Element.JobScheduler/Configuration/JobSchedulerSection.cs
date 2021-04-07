using System.Configuration;

namespace Element.JobScheduler.Configuration
{
    public class JobSchedulerSection : ConfigurationSection
    {
        [ConfigurationProperty("Jobs", IsDefaultCollection = true)]
        public JobCollection Jobs
        {
            get
            {
                JobCollection jobCollection = (JobCollection)base["Jobs"];

                return jobCollection;
            }
        }
    }

    public class JobCollection : ConfigurationElementCollection
    {
        public JobCollection()
        {
            JobConfigElement element = (JobConfigElement)CreateNewElement();
            if (!string.IsNullOrEmpty(element.Name))
            {
                BaseAdd(element);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JobConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element) => ((JobConfigElement)element).Name;

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        public JobConfigElement this[int index] => (JobConfigElement)BaseGet(index);

        new public JobConfigElement this[string index] => (JobConfigElement)BaseGet(index);

        protected override string ElementName
        {
            get { return "Job"; }
        }
    }

    public class JobConfigElement : ConfigurationElement
    {
        public JobConfigElement() { }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("schedule", IsRequired = true, DefaultValue = "* * * * *")]
        public string Schedule
        {
            get { return (string)this["schedule"]; }
            set { this["schedule"] = value; }
        }
    }
}
