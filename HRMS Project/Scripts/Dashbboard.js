$(document).ready(function () {
    loadNavbarData();
    loadSidebarMenus();
    loadDashboadData();
    leavepiechart();
    // Normal calendar
    loadAttendanceCalendarGeneric({
        calendarId: 'fullcalendar',
        eventsUrl: '/Attendance/GetAttendanceEvents',
        isDashboard: window.location.pathname.toLowerCase() === '/dashboard' ||
            window.location.pathname.toLowerCase() === '/dashboard/index'
    });

    // Selected employee calendar
    loadAttendanceCalendarGeneric({
        calendarId: 'fullcalendarselectedemp',
        eventsUrl: '/Attendance/Attendanceselectedemp',
        dropdownId: 'employeeDropdown'
    });


    fetch('/Attendance/GetTodayMode')
        .then(response => response.json())
        .then(data => {
            if (data.modeName) {
                document.getElementById("modeName").value = data.modeName;
                const btn = document.getElementById("attendanceButton");
                btn.innerText = data.modeName;
                btn.classList.add("selected");
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

function selectAttendance(mode) {
    document.getElementById("modeName").value = mode;
    const btn = document.getElementById("attendanceButton");
    btn.innerText = mode;
    btn.classList.add("selected");

    const token = document.querySelector('#modeForm input[name="__RequestVerificationToken"]').value;

    $.ajax({
        url: '/Attendance/SetMode',
        type: 'POST',
        data: {
            modeName: mode,
            __RequestVerificationToken: token
        }
    });
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

//calendar

//calendar
function loadAttendanceCalendarGeneric({
    calendarId,
    eventsUrl,
    dropdownId = null,
    isDashboard = false
}) {
    const calendarEl = document.getElementById(calendarId);
    if (!calendarEl) return;

    const empDropdown = dropdownId ? document.getElementById(dropdownId) : null;

    let calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: isDashboard
            ? { left: '', center: 'title', right: '' }
            : { left: 'prev,next today', center: 'title', right: '' },

        events: function (fetchInfo, successCallback, failureCallback) {
            let url = eventsUrl;
            if (empDropdown) {
                url += (url.includes("?") ? "&" : "?") + `empId=${empDropdown.value}`;
            }

            fetch(url)
                .then(response => response.json())
                .then(data => successCallback(data))
                .catch(err => failureCallback(err));
        },

        dateClick: function (info) {
            const clickedDate = info.dateStr;
            const eventsOnDate = calendar.getEvents().filter(e => e.startStr === clickedDate);

            let detailText = null;
            if (eventsOnDate.length > 0) {
                const uniqueDetails = [...new Set(eventsOnDate.map(e => e.extendedProps.fullStatus))];
                detailText = uniqueDetails.join("<br>");
            }

            showReportBox(clickedDate, detailText, info.dayEl);
        }
    });

    // Dashboard-specific adjustments
    if (isDashboard) {
        calendar.setOption('contentHeight', '100%');
        calendar.setOption('expandRows', true);
    }

    calendar.render();

    // Refresh when dropdown changes
    if (empDropdown) {
        empDropdown.addEventListener("change", function () {
            calendar.refetchEvents();
        });
    }

    // Hide report box on outside click
    document.addEventListener('click', function (e) {
        const reportBox = document.getElementById('reportBox');
        if (reportBox.style.display === 'block' &&
            !reportBox.contains(e.target) &&
            !calendarEl.contains(e.target)) {
            reportBox.style.display = 'none';
        }
    });
}

function showReportBox(date, status, dayCell) {
    const reportBox = document.getElementById('reportBox');
    const reportDate = document.getElementById('reportDate');
    const reportStatusRow = document.getElementById('reportStatusRow');
    const reportStatus = document.getElementById('reportStatus');

    reportDate.textContent = date;

    if (status) {
        reportStatus.innerHTML = status;
        reportStatusRow.style.display = 'block';
    } else {
        reportStatusRow.style.display = 'none';
    }

    const rect = dayCell.getBoundingClientRect();
    const scrollTop = window.scrollY || document.documentElement.scrollTop;
    const scrollLeft = window.scrollX || document.documentElement.scrollLeft;

    reportBox.style.top = (rect.top + scrollTop - reportBox.offsetHeight - 5) + "px";
    reportBox.style.left = (rect.left + scrollLeft) + "px";
    reportBox.style.display = 'block';
}
