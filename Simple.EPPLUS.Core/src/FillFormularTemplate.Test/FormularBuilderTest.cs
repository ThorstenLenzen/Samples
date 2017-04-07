using System;
using System.IO;
using Xunit;

namespace Toto.Simple.EPPLUS.Core.Test
{
    public class FormularBuilderTest
    {
        private readonly string _sourcePath;
        private readonly string _targetPath;

        public FormularBuilderTest()
        {
            _sourcePath = AppContext.BaseDirectory + @"\ErstmeldungKritischeInfrastrukturTemplate.xlsx";
            _targetPath = AppContext.BaseDirectory + @"\Formular.xlsx";

            if (File.Exists(_targetPath))
                File.Delete(_targetPath);
        }

        [Fact]
        public void BuildNewFormular_SetInputCorrect_ExcelFormularGetsCreated()
        {
            // Act
            FormularBuilder
                .NewBuilder                
                .SetSourceFile(_sourcePath)
                .SetTargetFile(_targetPath)
                .SetCompany("NGD - Unternehmens ID 1eb49")
                .SetSector("Elektrizität")
                .SetAffectedSite("SWD - Kraftwerk Lausward")
                .SetAffectedSiteType("Erzeugungsanlage mit Wärmeauskopplung (Kraftwerke Lausward")
                .SetAffectedService("Stromversorgung")
                .SetContactName("Sven Hellenbrand")
                .SetContactEmail("shellenbrand@swd-ag.de")
                .SetContactPhone("0211 821 4435")
                .SetSituationAssessment(SituationAssessments.Orange)
                .SetFunctionalityAffection("Lorem ipsum und so weiter :-)...")
                .IsSystemComprehesive(YesNoDecision.No)
                .SetAffectedItSystem("Lorem ipsum und so weiter :-)...")
                .SetCause("Lorem ipsum und so weiter :-)...")
                .SetStartTime(DateTime.Now)
                .SetEndTime(DateTime.Now + TimeSpan.FromHours(2))
                .IsOngoing(YesNoDecision.Yes)
                .Build();

            // Assert
            Assert.True(File.Exists(_targetPath));
        }

        [Fact]
        public void BuildNewFormular_SetTargetFileWithNameNull_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder                    
                    .SetTargetFile(null);
            });

            // Assert
            Assert.Equal("targetFileName must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetTargetFileWithNameEmpty_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder                    
                    .SetTargetFile("");
            });

            // Assert
            Assert.Equal("targetFileName must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetSourceFileWithNameNull_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder                    
                    .SetSourceFile(null);
            });

            // Assert
            Assert.Equal("sourceFileName must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetSourceFileWithNameEmpty_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder                    
                    .SetSourceFile("");
            });

            // Assert
            Assert.Equal("sourceFileName must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_BuildWithoutSettingSourceFile_FormularBuilderExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<FormularBuilderException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetTargetFile(_targetPath)
                    .Build();
            });

            // Assert
            Assert.Equal("Source file was not set.", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_BuildWithoutSettingTargetFile_FormularBuilderExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<FormularBuilderException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetSourceFile(_sourcePath)
                    .Build();
            });

            // Assert
            Assert.Equal("Target file was not set.", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetSourceFileToNonExisting_FormularBuilderExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<FormularBuilderException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetSourceFile(_sourcePath + "s")
                    .SetTargetFile(_targetPath)
                    .Build();
            });

            // Assert
            Assert.Equal($"Source file could not be found under {_sourcePath}s.", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetSectorWithNameNull_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetSector(null);
            });

            // Assert
            Assert.Equal("sector must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetSectorWithNameEmpty_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetSector("");
            });

            // Assert
            Assert.Equal("sector must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetCompanytWithNameNull_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetCompany(null);
            });

            // Assert
            Assert.Equal("company must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetCompanyWithNameEmpty_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetCompany("");
            });

            // Assert
            Assert.Equal("company must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetAffectedSiteWithNameNull_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetAffectedSite(null);
            });

            // Assert
            Assert.Equal("affectedSite must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetAffectedSiteWithNameEmpty_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetAffectedSite("");
            });

            // Assert
            Assert.Equal("affectedSite must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetAffectedSiteTypeWithNameNull_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetAffectedSiteType(null);
            });

            // Assert
            Assert.Equal("affectedSiteType must not be null or empty!", ex.Message);
        }

        [Fact]
        public void BuildNewFormular_SetAffectedSiteTypeWithNameEmpty_ArgumentExceptionGetsThrown()
        {
            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                FormularBuilder
                    .NewBuilder
                    .SetAffectedSiteType("");
            });

            // Assert
            Assert.Equal("affectedSiteType must not be null or empty!", ex.Message);
        }
    }
}
