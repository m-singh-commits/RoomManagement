<template>
    <div>
        <div class="text-2xl font-bold mb-4">
            Room Management - Day View
        </div>
        <div class="mb-4">
            <Button label="Calendar" icon="ic:outline-calendar-month" @click="onClickCalendar" />
        </div>
        <DayPilotCalendar :config="config" ref="calendarRef" />
    </div>
</template>

<script setup lang="ts">
import { DayPilot, DayPilotCalendar } from '@daypilot/daypilot-lite-vue'
import { ref, onMounted } from 'vue'

const config = ref({
    viewType: 'Resources',
    locale: 'en-us',
    headerHeight: 30,
    cellHeight: 30,
    businessBeginsHour: 6,
    businessEndsHour: 21,
    timeRangeSelectedHandling: 'Enabled',
    onTimeRangeSelected: async (args) => {
        const modal = await DayPilot.Modal.prompt('Create a new event:', 'Event 1')
        const calendar = args.control
        calendar.clearSelection()
        if (modal.canceled) { return }
        calendar.events.add({
            start: args.start,
            end: args.end,
            id: DayPilot.guid(),
            text: modal.result,
            resource: args.resource
        })
    },
    eventDeleteHandling: 'Update',
    onEventDeleted: (args) => {
        console.log('Event deleted: ' + args.e.text())
    },
    eventMoveHandling: 'Update',
    onEventMoved: (args) => {
        console.log('Event moved: ' + args.e.text())
    },
    eventResizeHandling: 'Update',
    onEventResized: (args) => {
        console.log('Event resized: ' + args.e.text())
    },
    eventClickHandling: 'ContextMenu',
    eventRightClickHandling: 'ContextMenu',
    contextMenu: new DayPilot.Menu({
        items: [
            {
                text: 'Delete', onClick: (args) => {
                    const dp = args.source.calendar
                    dp.events.remove(args.source)
                }
            }
        ]
    }),
})

const calendarRef = ref(null)

const loadEvents = () => {
    const events = [
        {
            id: 1,
            start: DayPilot.Date.today().addHours(12),
            end: DayPilot.Date.today().addHours(14),
            text: 'Event 1',
            resource: 'GA'
        },
        {
            id: 2,
            start: DayPilot.Date.today().addHours(9),
            end: DayPilot.Date.today().addHours(10),
            text: 'Event 2',
            resource: 'R1'
        }
    ]

    config.value.events = events
}

const loadResources = () => {
    const resources = [
        { name: 'Resource 1', id: 'R1' },
        { name: 'Resource 2', id: 'R2' },
        { name: 'Resource 3', id: 'R3' },
        { name: 'Resource 4', id: 'R4' },
        { name: 'Resource 5', id: 'R5' },
        { name: 'Resource 6', id: 'R6' },
        { name: 'Resource 7', id: 'R7' },
        { name: 'Resource 8', id: 'R8' }
    ]

    config.value.columns = resources
}

const onClickCalendar = () => navigateTo('/')

onMounted(() => {
    loadEvents()
    loadResources()
})
</script>
