﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Web.Controllers;

namespace VentureManagement.Web.Areas.Report.Controllers
{
    public class ThreatCaseReportBaseController : BaseController
    {
        protected readonly InterfaceThreatCaseService _threatCaseService;

        protected ThreatCaseReportBaseController()
        {
            _threatCaseService = new ThreatCaseService(_currentOrgList);
        }
    }
}
