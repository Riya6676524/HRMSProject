$(document).ready(function () {
    loadNavbarData();
    loadSidebarMenus();
    loadDashboadData();
    leavepiechart();
    loadAttendanceCalendar();

    fetch('/Attendance/GetTodayMode')
        .then(response => response.json())
        .then(data => {
            if (data.modeName) {
                document.getElementById("modeName").value = data.modeName;
                document.getElementById("attendanceButton").innerText = data.modeName;
            }
        });
});

//Menu click 
$(document).on('click', '.menu-list a', function () {
    const url = $(this).attr('href');
    window.location.href = url;
});

//Sidebar
function loadSidebarMenus() {
    $.getJSON('/Dashboard/GetMenus', function (menus) {
        let html = '';
        const currentPath = window.location.pathname.toLowerCase();
        const parentMenus = menus.filter(m => m.ParentMenuID === null);
        let openMenuIds = JSON.parse(localStorage.getItem("openMenuIds") || "[]");

        parentMenus.forEach(menu => {
            const subMenus = menus.filter(m => m.ParentMenuID === menu.MenuID);
            let isActiveParent = false;

            if (subMenus.length > 0) {
                let submenuHtml = '';
                subMenus.forEach(sub => {
                    const link = `/${sub.ControllerName}/${sub.ActionName}`.toLowerCase();
                    const isActive = currentPath === link;
                    if (isActive) {
                        isActiveParent = true;
                        if (!openMenuIds.includes(menu.MenuID)) {
                            openMenuIds.push(menu.MenuID);
                        }
                    }
                    submenuHtml += `<li>
                        <a href="${link}" class="${isActive ? 'active' : ''}">
                            <i class="${sub.IconeClass}"></i> ${sub.MenuName}
                        </a>
                    </li>`;
                });

                const isOpen = openMenuIds.includes(menu.MenuID);
                html += `<li class="has-dropdown ${isOpen ? 'open' : ''}">
                    <a href="javascript:void(0)" onclick="toggleDropdown(this, ${menu.MenuID})">
                        <i class="${menu.IconeClass}"></i> ${menu.MenuName}
                    </a>
                    <ul class="submenu" style="${isOpen ? 'display:block;' : ''}">
                        ${submenuHtml}
                    </ul>
                </li>`;
            } else {
                const link = `/${menu.ControllerName}/${menu.ActionName}`.toLowerCase();
                const isActive = (
                    currentPath === link ||
                    (currentPath === '/dashboard' && link === '/dashboard/index')
                );
                html += `<li>
                    <a href="${link}" class="${isActive ? 'active' : ''}">
                        <i class="${menu.IconeClass}"></i> ${menu.MenuName}
                    </a>
                </li>`;
            }
        });

        localStorage.setItem("openMenuIds", JSON.stringify(openMenuIds));
        $('#menuList').html(html);
    });
}

//Dropdown Toggle
function toggleDropdown(element, menuId) {
    const parent = element.closest('li');
    const submenu = parent.querySelector('.submenu');
    let openMenuIds = JSON.parse(localStorage.getItem("openMenuIds") || "[]");

    if (submenu) {
        const isOpen = submenu.style.display === 'block';
        submenu.style.display = isOpen ? 'none' : 'block';
        parent.classList.toggle('open', !isOpen);

        if (!isOpen) {
            if (!openMenuIds.includes(menuId)) {
                openMenuIds.push(menuId);
            }
        } else {
            openMenuIds = openMenuIds.filter(id => id !== menuId);
        }

        localStorage.setItem("openMenuIds", JSON.stringify(openMenuIds));
    }
}

//Attendance Menu Toggle
function toggleAttendanceMenu(event) {
    event.stopPropagation();
    const menu = document.getElementById('attendanceOptions');
    menu.style.display = (menu.style.display === 'block') ? 'none' : 'block';
}

//Attendance Option Select
function selectAttendance(mode) {
    const today = new Date().toISOString().split("T")[0];



    //Update UI and submit
    document.getElementById("modeName").value = mode;
    document.getElementById("attendanceButton").innerText = mode;
    document.getElementById("modeForm").submit();
}

// Hide attendance dropdown on outside click
window.addEventListener('click', function () {
    document.getElementById('attendanceOptions').style.display = 'none';
});

// Dashboard Welcome Message
function loadDashboadData() {
    $.getJSON('/Dashboard/GetDashboardData', function (data) {
        $('#welcomeText').text(`Welcome, ${data.FirstName}`);
    }).fail(function () {
        $('#welcomeText').text('Welcome');
    });
}

// Navbar Data
function loadNavbarData() {
    $.getJSON('/Dashboard/GetNavbarData', function (data) {
        const middle = data.MiddleName && data.MiddleName.trim() !== '' ? ` ${data.MiddleName}` : '';
        $('#profileName').text(`${data.FirstName} ${middle} ${data.LastName}`);
        $('#profileEmpId').text(`ID: ${data.EmployeeID}`);
        $('#profileRol').text(`Role: ${data.RoleName}`);
    });
}
// Leave Chart
function leavepiechart() {
    $.getJSON('/Dashboard/GetLeaveChartData', function (data) {
        const ctx = document.getElementById('leaveChart');
        if (ctx) {
            new Chart(ctx.getContext('2d'), {
                type: 'pie',
                data: {
                    labels: ['Available Leave', 'Leave Taken'],
                    datasets: [{
                        data: [data.TotalAvailable, data.LeaveTaken],
                        backgroundColor: ['#28a745', '#dc3545']
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                font: {
                                    size: 13
                                }
                            }
                        }
                    }
                }
            });
        }
    });
}


//  Calendar
function loadAttendanceCalendar() {
    const calendarEl = document.getElementById('fullcalendar');
    if (!calendarEl) return;

    const path = window.location.pathname.toLowerCase();
    const isDashboard = path === '/dashboard' || path === '/dashboard/index';

    const calendarOptions = {
        initialView: 'dayGridMonth',
        headerToolbar: isDashboard
            ? { left: '', center: 'title', right: '' }
            : { left: 'prev,next today', center: 'title', right: '' },

        events: '/Attendance/GetAttendanceEvents',

        eventClick: function (info) {
            alert(`Attendance on ${info.event.startStr}: ${info.event.title}`);
        }
    };

    if (isDashboard) {
        calendarOptions.contentHeight = '100%';
        calendarOptions.expandRows = true;
    }

    const calendar = new FullCalendar.Calendar(calendarEl, calendarOptions);
    calendar.render();
}


