using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Controllers;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable InconsistentNaming

namespace VentureManagement.Web.Areas.Member.Controllers
{
    public class MemberBaseController : BaseController
    {
        //
        // GET: /Member/MemberBase/

        protected readonly InterfaceUserService _userService;
        protected readonly InterfaceOrganizationService _orgSerivce;
        protected readonly InterfaceRoleService _roleSerivce = new RoleService();
        protected readonly InterfaceOrganizationRelationService _orgrService;
        protected readonly InterfaceRoleRelationService _userRoleRelationService = new UserRoleRelationService();
        protected readonly InterfaceUserOrganizationRelationService _userOrgRelationService;

        protected MemberBaseController()
        {
            _userService = new UserService(_currentOrgList);
            _orgSerivce = new OrganizationService(_currentOrgList);
            _orgrService = new OrganizationRelationService(_currentOrgList);
            _userOrgRelationService = new UserOrganizationRelationService(_currentOrgList);
        }

    }
}
