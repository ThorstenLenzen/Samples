using System;
using System.IO;
using OfficeOpenXml;

namespace Toto.Simple.EPPLUS.Core
{
    public class FormularBuilder
    {
        private const string StringNotSet = "Not set";
        private readonly DateTime _dateTimeNotSet = DateTime.MinValue;

        private string _sourceFileName;
        private string _targetFileName;
        private string _company;
        private string _sector;
        private string _affectedSite;
        private string _affectedSiteType;
        private string _affectedService;
        private string _contactName;
        private string _contactEmail;
        private string _contactPhone;
        private SituationAssessments _situationAssessment;
        private string _functionalityAffection;
        private YesNoDecision _isSystemComprehesive;
        private string _affectedItSystem;
        private string _cause;
        private DateTime _startTime;
        private DateTime _endTime;
        private YesNoDecision _isOngoing;

        public FormularBuilder()
        {
            _sourceFileName = StringNotSet;
            _targetFileName = StringNotSet;
            _company = StringNotSet;
            _sector = StringNotSet;
            _affectedSite = StringNotSet;
            _affectedService = StringNotSet;
            _contactName = StringNotSet;
            _contactEmail = StringNotSet;
            _contactPhone = StringNotSet;
            _situationAssessment = SituationAssessments.Unknown;
            _functionalityAffection = StringNotSet;
            _isSystemComprehesive = YesNoDecision.Unknown;
            _affectedItSystem = StringNotSet;
            _cause = StringNotSet;
            _startTime = _dateTimeNotSet;
            _endTime = _dateTimeNotSet;
            _isOngoing = YesNoDecision.Unknown;
        }

        public static FormularBuilder NewBuilder => new FormularBuilder();

        public FormularBuilder SetSourceFile(string sourceFileName)
        {
            if (string.IsNullOrEmpty(sourceFileName))
                throw new ArgumentException($"{nameof(sourceFileName)} must not be null or empty!");

            _sourceFileName = sourceFileName;
            return this;
        }

        public FormularBuilder SetTargetFile(string targetFileName)
        {
            if(string.IsNullOrEmpty(targetFileName))
                throw new ArgumentException($"{nameof(targetFileName)} must not be null or empty!");

            _targetFileName = targetFileName;
            return this;
        }

        public FormularBuilder SetCompany(string company)
        {
            if (string.IsNullOrEmpty(company))
                throw new ArgumentException($"{nameof(company)} must not be null or empty!");

            _company = company;
            return this;
        }

        public FormularBuilder SetSector(string sector)
        {
            if (string.IsNullOrEmpty(sector))
                throw new ArgumentException($"{nameof(sector)} must not be null or empty!");

            _sector = sector;
            return this;
        }

        public FormularBuilder SetAffectedSite(string affectedSite)
        {
            if (string.IsNullOrEmpty(affectedSite))
                throw new ArgumentException($"{nameof(affectedSite)} must not be null or empty!");

            _affectedSite = affectedSite;
            return this;
        }

        public FormularBuilder SetAffectedSiteType(string affectedSiteType)
        {
            if (string.IsNullOrEmpty(affectedSiteType))
                throw new ArgumentException($"{nameof(affectedSiteType)} must not be null or empty!");

            _affectedSiteType = affectedSiteType;
            return this;
        }

        public FormularBuilder SetAffectedService(string affectedService)
        {
            if (string.IsNullOrEmpty(affectedService))
                throw new ArgumentException($"{nameof(affectedService)} must not be null or empty!");

            _affectedService = affectedService;
            return this;
        }

        public FormularBuilder SetContactName(string contactName)
        {
            if (string.IsNullOrEmpty(contactName))
                throw new ArgumentException($"{nameof(contactName)} must not be null or empty!");

            _contactName = contactName;
            return this;
        }

        public FormularBuilder SetContactEmail(string contactEmail)
        {
            if (string.IsNullOrEmpty(contactEmail))
                throw new ArgumentException($"{nameof(contactEmail)} must not be null or empty!");

            _contactEmail = contactEmail;
            return this;
        }

        public FormularBuilder SetContactPhone(string contactPhone)
        {
            if (string.IsNullOrEmpty(contactPhone))
                throw new ArgumentException($"{nameof(contactPhone)} must not be null or empty!");

            _contactPhone = contactPhone;
            return this;
        }

        public FormularBuilder SetSituationAssessment(SituationAssessments situationAssessment)
        {
            if (situationAssessment == SituationAssessments.Unknown)
                throw new ArgumentException($"Unknow is not an allowed value for {nameof(situationAssessment)}!");

            _situationAssessment = situationAssessment;
            return this;
        }

        public FormularBuilder SetFunctionalityAffection(string functionalityAffection)
        {
            if (string.IsNullOrEmpty(functionalityAffection))
                throw new ArgumentException($"{nameof(functionalityAffection)} must not be null or empty!");

            _functionalityAffection = functionalityAffection;
            return this;
        }

        public FormularBuilder IsSystemComprehesive(YesNoDecision isSystemComprehesive)
        {
            if (isSystemComprehesive == YesNoDecision.Unknown)
                throw new ArgumentException($"Unknown is not an allowed value for {nameof(isSystemComprehesive)}!");

            _isSystemComprehesive = isSystemComprehesive;
            return this;
        }

        public FormularBuilder SetAffectedItSystem(string affectedItSystem)
        {
            if (string.IsNullOrEmpty(affectedItSystem))
                throw new ArgumentException($"{nameof(affectedItSystem)} must not be null or empty!");

            _affectedItSystem = affectedItSystem;
            return this;
        }

        public FormularBuilder SetCause(string cause)
        {
            if (string.IsNullOrEmpty(cause))
                throw new ArgumentException($"{nameof(cause)} must not be null or empty!");

            _cause = cause;
            return this;
        }

        public FormularBuilder SetStartTime(DateTime startTime)
        {
            _startTime = startTime;
            return this;
        }

        public FormularBuilder SetEndTime(DateTime endTime)
        {
            _endTime = endTime;
            return this;
        }

        public FormularBuilder IsOngoing(YesNoDecision isOngoing)
        {
            if (isOngoing == YesNoDecision.Unknown)
                throw new ArgumentException($"Unknown is not an allowed value for {nameof(isOngoing)}!");

            _isOngoing = isOngoing;
            return this;
        }

        public void Build()
        {
            this.ValidateInput();

            var sourceFile = new FileInfo(_sourceFileName);
            var targetFile = new FileInfo(_targetFileName);

            if (targetFile.Exists)
                targetFile.Delete();

            using (var package = new ExcelPackage(sourceFile))
            {
                // do work here
                var sheet = package.Workbook.Worksheets["Meldung"];

                SheetHelper.SelectSheet(sheet)
                    .SetStringWithDefaultStyle(CellPositions.CompanyName, _company)
                    .SetStringWithDefaultStyle(CellPositions.Department, _sector)
                    .SetStringWithDefaultStyle(CellPositions.AffectedSite, _affectedSite)
                    .SetStringWithDefaultStyle(CellPositions.AffectedSiteType, _affectedSiteType)
                    .SetStringWithDefaultStyle(CellPositions.AffectedService, _affectedService)
                    .SetStringWithDefaultStyle(CellPositions.ContactName, _contactName)
                    .SetStringWithDefaultStyle(CellPositions.ContactMail, _contactEmail)
                    .SetStringWithDefaultStyle(CellPositions.ContactPhone, _contactPhone)
                    .SetStringWithDefaultStyle(CellPositions.SituationAssessment, _situationAssessment.GetStringValue())
                    .SetStringWithDefaultStyle(CellPositions.FunctionalityAffection, _functionalityAffection)
                    .SetStringWithDefaultStyle(CellPositions.IsSystemComprehesive, _isSystemComprehesive.GetStringValue())
                    .SetStringWithDefaultStyle(CellPositions.AffectedItSystem, _affectedItSystem)
                    .SetStringWithDefaultStyle(CellPositions.Cause, _cause)
                    .SetDateTimeOnTwoLinesCentered(CellPositions.StartTime, _startTime)
                    .SetDateTimeOnTwoLinesCentered(CellPositions.EndTime, _endTime)
                    .SetStringWithDefaultStyle(CellPositions.IsOngoing, _isOngoing.GetStringValue());

                package.SaveAs(targetFile);
            }
        }

        private void ValidateInput()
        {
            if (_sourceFileName == StringNotSet)
                throw new FormularBuilderException("Source file was not set.");

            if (!File.Exists(_sourceFileName))
                throw new FormularBuilderException($"Source file could not be found under {_sourceFileName}.");

            if (_targetFileName == StringNotSet)
                throw new FormularBuilderException("Target file was not set.");

            if (_company == StringNotSet)
                throw new FormularBuilderException("Company Name was not set.");

            if (_sector == StringNotSet)
                throw new FormularBuilderException("Department was not set.");

            if (_affectedSite == StringNotSet)
                throw new FormularBuilderException("Affected site was not set.");

            if (_affectedSiteType == StringNotSet)
                throw new FormularBuilderException("Affected site type was not set.");

            if (_affectedService == StringNotSet)
                throw new FormularBuilderException("Affected service was not set.");

            if (_contactName == StringNotSet)
                throw new FormularBuilderException("Contact name service was not set.");

            if (_contactEmail == StringNotSet)
                throw new FormularBuilderException("Contact email service was not set.");

            if (_contactPhone == StringNotSet)
                throw new FormularBuilderException("Contact phone service was not set.");

            if (_situationAssessment == SituationAssessments.Unknown)
                throw new FormularBuilderException("Situation assessment was not set.");

            if (_functionalityAffection == StringNotSet)
                throw new FormularBuilderException("Functionality affection was not set.");

            if (_isSystemComprehesive == YesNoDecision.Unknown)
                throw new FormularBuilderException("System comprehension was not set.");

            if (_affectedItSystem == StringNotSet)
                throw new FormularBuilderException("Affected IT system was not set.");

            if (_cause == StringNotSet)
                throw new FormularBuilderException("Cause was not set.");

            if (_startTime == _dateTimeNotSet)
                throw new FormularBuilderException("Start time was not set.");

            if (_endTime == _dateTimeNotSet)
                throw new FormularBuilderException("End time was not set.");

            if (_isOngoing == YesNoDecision.Unknown)
                throw new FormularBuilderException("Is ongoing was not set.");
        }
    }
}
