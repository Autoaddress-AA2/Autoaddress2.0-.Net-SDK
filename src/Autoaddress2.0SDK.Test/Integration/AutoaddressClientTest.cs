﻿using Autoaddress.Autoaddress2_0.Model;
using NUnit.Framework;

namespace Autoaddress.Autoaddress2_0.Test.Integration
{
    [TestFixture]
    public class AutoaddressClientTest
    {
        [Test]
        public void FindAddress_8SilverBirchesDunboyne_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address: address, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);
            
            var response = autoaddressClient.FindAddress(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeAppended, response.Result);
            Assert.AreEqual("A86VC04", response.Postcode);
        }

        [Test]
        public void FindAddress_8SilverBirchesDunboyneA86VC04_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne, A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address: address, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = autoaddressClient.FindAddress(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeValidated, response.Result);
            Assert.AreEqual("A86VC04", response.Postcode);
        }

        [Test]
        public void FindAddress_8SilverBirchesDunboyneA86VC05_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne, A86VC05";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address: address, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = autoaddressClient.FindAddress(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeAmended, response.Result);
            Assert.AreEqual("A86VC04", response.Postcode);
        }

        [Test]
        public void FindAddress_9SilverBirchesDunboyneA86VC04_ReturnsValidResponse()
        {
            const string address = "9 Silver Birches, Dunboyne, A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address: address, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = autoaddressClient.FindAddress(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.AddressAmendedToMatchPostcode, response.Result);
            Assert.AreEqual("A86VC04", response.Postcode);
        }

        [Test]
        public void FindAddress_8SilverBirchesDunboyneInvalidLicenceKey_ThrowsAutoaddressException()
        {
            const string licenceKey = "InvalidLicenceKey";
            const string address = "8 Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient(licenceKey);
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            AutoaddressException autoaddressException = Assert.Throws<AutoaddressException>(() => autoaddressClient.FindAddress(request));
            Assert.AreEqual(ErrorType.InvalidLicenceKey, autoaddressException.ErrorType);
        }

        [Test]
        public void FindAddress_8SilverBirchesDunboyneUseKeyFromAppConfig_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            var response = autoaddressClient.FindAddress(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeAppended, response.Result);
            Assert.AreEqual("A86VC04", response.Postcode);
        }

        [Test]
        public void FindAddress_SilverBirchesDunboyne_ReturnsValidResponse()
        {
            const string address = "Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            var response = autoaddressClient.FindAddress(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Options);
        }

        [Test]
        public void FindAddress_SilverBirchesDunboyneThenSelectFirstOption_ReturnsValidResponses()
        {
            const string address = "Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            var firstResponse = autoaddressClient.FindAddress(request);

            Assert.NotNull(firstResponse);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.IncompleteAddressEntered, firstResponse.Result);
            Assert.NotNull(firstResponse.Options);
            var option = firstResponse.Options[0];
            var link = option.Links[0];
            
            var secondResponse = autoaddressClient.FindAddress(link);
            Assert.NotNull(secondResponse);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.IncompleteAddressEntered, secondResponse.Result);
            Assert.NotNull(secondResponse.Options);
        }

        [Test]
        public void FindAddress_SilverBirchesDunboyneThenSelectSelfLink_ReturnsValidResponses()
        {
            const string address = "Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            var firstResponse = autoaddressClient.FindAddress(request);

            Assert.NotNull(firstResponse);
            Assert.NotNull(firstResponse.Links);
            Assert.Greater(firstResponse.Links.Length, 0);
            var link = firstResponse.Links[0];

            var secondResponse = autoaddressClient.FindAddress(link);

            Assert.NotNull(secondResponse);
            Assert.AreEqual(firstResponse.Result, secondResponse.Result);
            Assert.AreEqual(firstResponse.AddressId, secondResponse.AddressId);
        }

        [Test]
        public async void FindAddressAsync_8SilverBirchesDunboyne_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            var response = await autoaddressClient.FindAddressAsync(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeAppended, response.Result);
            Assert.AreEqual("A86VC04", response.Postcode);
        }

        [Test]
        public void FindAddressAsync_8SilverBirchesDunboyneInvalidLicenceKey_ThrowsAutoaddressException()
        {
            const string licenceKey = "InvalidLicenceKey";
            const string address = "8 Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient(licenceKey);
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            Assert.Throws<AutoaddressException>(async () => await autoaddressClient.FindAddressAsync(request));
        }

        [Test]
        public async void FindAddressAsync_SilverBirchesDunboyne_ReturnsValidResponse()
        {
            const string address = "Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress.Autoaddress2_0.Model.FindAddress.Request(address, Language.EN, Country.IE, 20, false, null);

            var response = await autoaddressClient.FindAddressAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Options);
        }

        [Test]
        public void PostcodeLookup_A86VC04_ReturnsValidResponse()
        {
            const string postcode = "A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var response = autoaddressClient.PostcodeLookup(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, response.Result);
            Assert.AreEqual(postcode, response.Postcode);
            Assert.NotNull(response.PostalAddress);
            Assert.AreEqual(4, response.PostalAddress.Length);
            Assert.AreEqual("8 SILVER BIRCHES", response.PostalAddress[0]);
            Assert.AreEqual("MILLFARM", response.PostalAddress[1]);
            Assert.AreEqual("DUNBOYNE", response.PostalAddress[2]);
            Assert.AreEqual("CO. MEATH", response.PostalAddress[3]);
        }

        [Test]
        public void PostcodeLookup_A86VC04ThenSelectSelfLink_ReturnsValidResponse()
        {
            const string postcode = "A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var firstResponse = autoaddressClient.PostcodeLookup(request);
            
            Assert.NotNull(firstResponse);
            Assert.NotNull(firstResponse.Links);
            Assert.Greater(firstResponse.Links.Length, 0);
            var link = firstResponse.Links[0];

            var secondResponse = autoaddressClient.PostcodeLookup(link);

            Assert.NotNull(secondResponse);
            Assert.AreEqual(firstResponse.Result, secondResponse.Result);
            Assert.AreEqual(firstResponse.AddressId, secondResponse.AddressId);
        }

        [Test]
        public void PostcodeLookup_D08XY00_ReturnsValidResponse()
        {
            const string postcode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var response = autoaddressClient.PostcodeLookup(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, response.Result);
            Assert.AreEqual(postcode, response.Postcode);
            Assert.NotNull(response.PostalAddress);
            Assert.AreEqual(3, response.PostalAddress.Length);
            Assert.AreEqual("4 INNS COURT", response.PostalAddress[0]);
            Assert.AreEqual("WINETAVERN STREET", response.PostalAddress[1]);
            Assert.AreEqual("DUBLIN 8", response.PostalAddress[2]);
            Assert.IsNotNull(response.Options);
            Assert.AreEqual(3, response.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[2].DisplayName);
        }

        [Test]
        public void PostcodeLookup_D08XY00ThenSelectGammaFromOptions_ReturnsValidResponses()
        {
            const string postcode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var firstResponse = autoaddressClient.PostcodeLookup(request);

            Assert.NotNull(firstResponse);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, firstResponse.Result);
            Assert.AreEqual(postcode, firstResponse.Postcode);
            Assert.NotNull(firstResponse.PostalAddress);
            Assert.AreEqual(3, firstResponse.PostalAddress.Length);
            Assert.AreEqual("4 INNS COURT", firstResponse.PostalAddress[0]);
            Assert.AreEqual("WINETAVERN STREET", firstResponse.PostalAddress[1]);
            Assert.AreEqual("DUBLIN 8", firstResponse.PostalAddress[2]);
            Assert.IsNotNull(firstResponse.Options);
            Assert.AreEqual(3, firstResponse.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", firstResponse.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", firstResponse.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", firstResponse.Options[2].DisplayName);
            Assert.NotNull(firstResponse.Options[2].Links);
            Assert.Greater(firstResponse.Options[2].Links.Length, 0);
            Assert.NotNull(firstResponse.Options[2].Links[0]);

            var secondResponse = autoaddressClient.PostcodeLookup(firstResponse.Options[2].Links[0]);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, firstResponse.Result);
            Assert.AreEqual(postcode, firstResponse.Postcode);
            Assert.AreEqual(AddressType.Organisation, secondResponse.AddressType);
        }

        [Test]
        public async void PostcodeLookupAsync_A86VC04_ReturnsValidResponse()
        {
            const string postcode = "A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var response = await autoaddressClient.PostcodeLookupAsync(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, response.Result);
            Assert.AreEqual(postcode, response.Postcode);
            Assert.NotNull(response.PostalAddress);
            Assert.AreEqual(4, response.PostalAddress.Length);
            Assert.AreEqual("8 SILVER BIRCHES", response.PostalAddress[0]);
            Assert.AreEqual("MILLFARM", response.PostalAddress[1]);
            Assert.AreEqual("DUNBOYNE", response.PostalAddress[2]);
            Assert.AreEqual("CO. MEATH", response.PostalAddress[3]);
        }

        [Test]
        public async void PostcodeLookupAsync_D08XY00_ReturnsValidResponse()
        {
            const string postcode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var response = await autoaddressClient.PostcodeLookupAsync(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, response.Result);
            Assert.AreEqual(postcode, response.Postcode);
            Assert.NotNull(response.PostalAddress);
            Assert.AreEqual(3, response.PostalAddress.Length);
            Assert.AreEqual("4 INNS COURT", response.PostalAddress[0]);
            Assert.AreEqual("WINETAVERN STREET", response.PostalAddress[1]);
            Assert.AreEqual("DUBLIN 8", response.PostalAddress[2]);
            Assert.IsNotNull(response.Options);
            Assert.AreEqual(3, response.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[2].DisplayName);
        }

        [Test]
        public async void PostcodeLookupAsync_D08XY00ThenSelectGammaFromOptions_ReturnsValidResponses()
        {
            const string postcode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.PostcodeLookup.Request(postcode, Language.EN, Country.IE, 20);

            var firstResponse = await autoaddressClient.PostcodeLookupAsync(request);

            Assert.NotNull(firstResponse);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, firstResponse.Result);
            Assert.AreEqual(postcode, firstResponse.Postcode);
            Assert.NotNull(firstResponse.PostalAddress);
            Assert.AreEqual(3, firstResponse.PostalAddress.Length);
            Assert.AreEqual("4 INNS COURT", firstResponse.PostalAddress[0]);
            Assert.AreEqual("WINETAVERN STREET", firstResponse.PostalAddress[1]);
            Assert.AreEqual("DUBLIN 8", firstResponse.PostalAddress[2]);
            Assert.IsNotNull(firstResponse.Options);
            Assert.AreEqual(3, firstResponse.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", firstResponse.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", firstResponse.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", firstResponse.Options[2].DisplayName);
            Assert.NotNull(firstResponse.Options[2].Links);
            Assert.Greater(firstResponse.Options[2].Links.Length, 0);
            Assert.NotNull(firstResponse.Options[2].Links[0]);

            var secondResponse = await autoaddressClient.PostcodeLookupAsync(firstResponse.Options[2].Links[0]);
            Assert.AreEqual(Autoaddress2_0.Model.PostcodeLookup.ReturnCode.ValidPostcode, firstResponse.Result);
            Assert.AreEqual(postcode, firstResponse.Postcode);
            Assert.AreEqual(AddressType.Organisation, secondResponse.AddressType);
        }

        [Test]
        public void VerifyAddress_8SilverBirchesDunboyneA86VC04_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne";
            const string postcode = "A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.VerifyAddress.Request(postcode, address, Language.EN, Country.IE);

            var response = autoaddressClient.VerifyAddress(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.VerifyAddress.ReturnCode.AddressAndEircodeMatch, response.Result);
            Assert.AreEqual(postcode, response.Postcode);
            Assert.NotNull(response.PostalAddress);
            Assert.AreEqual(4, response.PostalAddress.Length);
            Assert.AreEqual("8 SILVER BIRCHES", response.PostalAddress[0]);
            Assert.AreEqual("MILLFARM", response.PostalAddress[1]);
            Assert.AreEqual("DUNBOYNE", response.PostalAddress[2]);
            Assert.AreEqual("CO. MEATH", response.PostalAddress[3]);
        }

        [Test]
        public void VerifyAddress_8SilverBirchesDunboyneA86VC04ThenSelectSelfLink_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne";
            const string postcode = "A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.VerifyAddress.Request(postcode, address, Language.EN, Country.IE);

            var firstResponse = autoaddressClient.VerifyAddress(request);

            Assert.NotNull(firstResponse);
            Assert.NotNull(firstResponse.Links);
            Assert.Greater(firstResponse.Links.Length, 0);
            var link = firstResponse.Links[0];

            var secondResponse = autoaddressClient.VerifyAddress(link);

            Assert.NotNull(secondResponse);
            Assert.AreEqual(firstResponse.Result, secondResponse.Result);
            Assert.AreEqual(firstResponse.AddressId, secondResponse.AddressId);
        }

        [Test]
        public async void VerifyAddressAsync_8SilverBirchesDunboyneA86VC04_ReturnsValidResponse()
        {
            const string address = "8 Silver Birches, Dunboyne";
            const string postcode = "A86VC04";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.VerifyAddress.Request(postcode, address, Language.EN, Country.IE);

            var response = await autoaddressClient.VerifyAddressAsync(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.VerifyAddress.ReturnCode.AddressAndEircodeMatch, response.Result);
            Assert.AreEqual(postcode, response.Postcode);
            Assert.NotNull(response.PostalAddress);
            Assert.AreEqual(4, response.PostalAddress.Length);
            Assert.AreEqual("8 SILVER BIRCHES", response.PostalAddress[0]);
            Assert.AreEqual("MILLFARM", response.PostalAddress[1]);
            Assert.AreEqual("DUNBOYNE", response.PostalAddress[2]);
            Assert.AreEqual("CO. MEATH", response.PostalAddress[3]);
        }

        [Test]
        public void GetEcadData_1701984269_ReturnsValidResponse()
        {
            const int ecadId = 1701984269;
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.GetEcadData.Request(ecadId);

            var response = autoaddressClient.GetEcadData(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.GetEcadData.ReturnCode.EcadIdValid, response.Result);
            Assert.AreEqual(ecadId, response.EcadId);
        }

        [Test]
        public void GetEcadData_1701984269ThenSelectSelfLink_ReturnsValidResponses()
        {
            const int ecadId = 1701984269;
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.GetEcadData.Request(ecadId);

            var firstResponse = autoaddressClient.GetEcadData(request);

            Assert.NotNull(firstResponse);
            Assert.NotNull(firstResponse.Links);
            Assert.Greater(firstResponse.Links.Length, 0);
            var link = firstResponse.Links[0];

            var secondResponse = autoaddressClient.GetEcadData(link);

            Assert.NotNull(secondResponse);
            Assert.AreEqual(firstResponse.Result, secondResponse.Result);
            Assert.AreEqual(firstResponse.EcadId, secondResponse.EcadId);
        }

        [Test]
        public void GetEcadData_1200003223_ReturnsValidResponse()
        {
            const int ecadId = 1200003223;
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.GetEcadData.Request(ecadId);

            var response = autoaddressClient.GetEcadData(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.GetEcadData.ReturnCode.EcadIdValid, response.Result);
            Assert.AreEqual(ecadId, response.EcadId);
            Assert.NotNull(response.SpatialInfo);
            Assert.NotNull(response.SpatialInfo.Etrs89);
            Assert.NotNull(response.SpatialInfo.Etrs89.Location);
            Assert.Greater(response.SpatialInfo.Etrs89.Location.Latitude, 0);
            Assert.Less(response.SpatialInfo.Etrs89.Location.Longitude, 0);
            Assert.NotNull(response.SpatialInfo.Etrs89.BoundingBox);
            Assert.NotNull(response.SpatialInfo.Etrs89.BoundingBox.Min);
            Assert.NotNull(response.SpatialInfo.Etrs89.BoundingBox.Max);
            Assert.Greater(response.SpatialInfo.Etrs89.BoundingBox.Min.Latitude, 0);
            Assert.Less(response.SpatialInfo.Etrs89.BoundingBox.Min.Longitude, 0);
            Assert.Greater(response.SpatialInfo.Etrs89.BoundingBox.Max.Latitude, 0);
            Assert.Less(response.SpatialInfo.Etrs89.BoundingBox.Max.Longitude, 0);
            Assert.NotNull(response.SpatialInfo.Ing);
            Assert.NotNull(response.SpatialInfo.Etrs89.Location);
            Assert.Greater(response.SpatialInfo.Etrs89.Location.Latitude, 0);
            Assert.Less(response.SpatialInfo.Etrs89.Location.Longitude, 0);
            Assert.NotNull(response.SpatialInfo.Ing);
            Assert.NotNull(response.SpatialInfo.Ing.BoundingBox);
            Assert.NotNull(response.SpatialInfo.Ing.BoundingBox.Min);
            Assert.NotNull(response.SpatialInfo.Ing.BoundingBox.Max);
            Assert.Greater(response.SpatialInfo.Ing.BoundingBox.Min.Easting, 0);
            Assert.Greater(response.SpatialInfo.Ing.BoundingBox.Min.Northing, 0);
            Assert.Greater(response.SpatialInfo.Ing.BoundingBox.Max.Easting, 0);
            Assert.Greater(response.SpatialInfo.Ing.BoundingBox.Max.Northing, 0);
            Assert.NotNull(response.SpatialInfo.Itm);
            Assert.NotNull(response.SpatialInfo.Itm.BoundingBox);
            Assert.NotNull(response.SpatialInfo.Itm.BoundingBox.Min);
            Assert.NotNull(response.SpatialInfo.Itm.BoundingBox.Max);
            Assert.Greater(response.SpatialInfo.Itm.BoundingBox.Min.Easting, 0);
            Assert.Greater(response.SpatialInfo.Itm.BoundingBox.Min.Northing, 0);
            Assert.Greater(response.SpatialInfo.Itm.BoundingBox.Max.Easting, 0);
            Assert.Greater(response.SpatialInfo.Itm.BoundingBox.Max.Northing, 0);
        }

        [Test]
        public async void GetEcadDataAsync_1701984269_ReturnsValidResponse()
        {
            const int ecadId = 1701984269;
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.GetEcadData.Request(ecadId);

            var response = await autoaddressClient.GetEcadDataAsync(request);

            Assert.NotNull(response);
            Assert.AreEqual(Autoaddress2_0.Model.GetEcadData.ReturnCode.EcadIdValid, response.Result);
            Assert.AreEqual(ecadId, response.EcadId);
        }

        [Test]
        public void AutoComplete_SilverBirchesDunboyne_ReturnsValidResponse()
        {
            const string address = "Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.AutoComplete.Request(address: address, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = autoaddressClient.AutoComplete(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Options);
            Assert.AreEqual(1, response.Options.Length);
            Assert.AreEqual("SILVER BIRCHES, MILLFARM, DUNBOYNE, CO. MEATH", response.Options[0].DisplayName);
        }

        [Test]
        public async void AutoCompleteAsync_SilverBirchesDunboyne_ReturnsValidResponse()
        {
            const string address = "Silver Birches, Dunboyne";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.AutoComplete.Request(address: address, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = await autoaddressClient.AutoCompleteAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Options);
            Assert.AreEqual(1, response.Options.Length);
            Assert.AreEqual("SILVER BIRCHES, MILLFARM, DUNBOYNE, CO. MEATH", response.Options[0].DisplayName);
        }

        [Test]
        public void AutoComplete_D08XY00_ReturnsValidResponse()
        {
            const string eircode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.AutoComplete.Request(address: eircode, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = autoaddressClient.AutoComplete(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Options);
            Assert.AreEqual(3, response.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[2].DisplayName);
        }

        [Test]
        public async void AutoCompleteAsync_D08XY00_ReturnsValidResponse()
        {
            const string eircode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.AutoComplete.Request(address: eircode, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var response = await autoaddressClient.AutoCompleteAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Options);
            Assert.AreEqual(3, response.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", response.Options[2].DisplayName);
        }

        [Test]
        public void AutoCompleteThenFindAddress_D08XY00_ReturnsValidResponse()
        {
            const string eircode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.AutoComplete.Request(address: eircode, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var autoCompleteResponse = autoaddressClient.AutoComplete(request);

            Assert.NotNull(autoCompleteResponse);
            Assert.NotNull(autoCompleteResponse.Options);
            Assert.AreEqual(3, autoCompleteResponse.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", autoCompleteResponse.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", autoCompleteResponse.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", autoCompleteResponse.Options[2].DisplayName);

            var link = autoCompleteResponse.Options[1].Links[0];

            var findAddressResponse = autoaddressClient.FindAddress(link);
            
            Assert.NotNull(findAddressResponse);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeAppended, findAddressResponse.Result);
            Assert.AreEqual(eircode, findAddressResponse.Postcode);
            Assert.NotNull(findAddressResponse.PostalAddress);
            Assert.AreEqual(4, findAddressResponse.PostalAddress.Length);
            Assert.AreEqual("AUTOADDRESS", findAddressResponse.PostalAddress[0]);
            Assert.AreEqual("4 INNS COURT", findAddressResponse.PostalAddress[1]);
            Assert.AreEqual("WINETAVERN STREET", findAddressResponse.PostalAddress[2]);
            Assert.AreEqual("DUBLIN 8", findAddressResponse.PostalAddress[3]);
        }

        [Test]
        public async void AutoCompleteAsyncThenFindAddressAsync_D08XY00_ReturnsValidResponse()
        {
            const string eircode = "D08XY00";
            var autoaddressClient = new AutoaddressClient();
            var request = new Autoaddress2_0.Model.AutoComplete.Request(address: eircode, language: Language.EN, country: Country.IE, limit: 20, isVanityMode: false, addressProfileName: null);

            var autoCompleteResponse = await autoaddressClient.AutoCompleteAsync(request);

            Assert.NotNull(autoCompleteResponse);
            Assert.NotNull(autoCompleteResponse.Options);
            Assert.AreEqual(3, autoCompleteResponse.Options.Length);
            Assert.AreEqual("4 INNS COURT, WINETAVERN STREET, DUBLIN 8", autoCompleteResponse.Options[0].DisplayName);
            Assert.AreEqual("AUTOADDRESS, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", autoCompleteResponse.Options[1].DisplayName);
            Assert.AreEqual("GAMMA, 4 INNS COURT, WINETAVERN STREET, DUBLIN 8", autoCompleteResponse.Options[2].DisplayName);

            var link = autoCompleteResponse.Options[1].Links[0];

            var findAddressResponse = await autoaddressClient.FindAddressAsync(link);
            
            Assert.NotNull(findAddressResponse);
            Assert.AreEqual(Autoaddress.Autoaddress2_0.Model.FindAddress.ReturnCode.PostcodeAppended, findAddressResponse.Result);
            Assert.AreEqual(eircode, findAddressResponse.Postcode);
            Assert.NotNull(findAddressResponse.PostalAddress);
            Assert.AreEqual(4, findAddressResponse.PostalAddress.Length);
            Assert.AreEqual("AUTOADDRESS", findAddressResponse.PostalAddress[0]);
            Assert.AreEqual("4 INNS COURT", findAddressResponse.PostalAddress[1]);
            Assert.AreEqual("WINETAVERN STREET", findAddressResponse.PostalAddress[2]);
            Assert.AreEqual("DUBLIN 8", findAddressResponse.PostalAddress[3]);
        }
    }
}
