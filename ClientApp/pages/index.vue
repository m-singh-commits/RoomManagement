<template>
    <div>
        <div class="text-2xl font-bold mb-4">
            Room Management Calendar
        </div>
        <div class="flex space-x-4 mb-4">
            <Button icon="ooui:arrow-previous-ltr" label="Previous Month" @click="onClickPreviousMonth" />
            <Button icon="ooui:arrow-previous-rtl" label="Next Month" @click="onClickNextMonth" />
        </div>
        <div class="text-2xl font-bold mb-4 text-center">
            {{ months[currentDate.getMonth()] }} - {{ currentDate.getFullYear() }}
        </div>
        <DayPilotMonth :config="config" ref="monthRef" />
        <div class="flex space-x-4 mb-4">
            <Button label="Add Event" class="mt-4" @click="modal.open({eventInfo: {}})"/>
        </div>
    </div>

</template>

<script setup>
import { DayPilot, DayPilotMonth } from '@daypilot/daypilot-lite-vue'
import { ref, onMounted } from 'vue'
import {EventForm} from '#components'

const event = ref();

const eventFormRef = ref();
const toast = useToast;
const currentDate = ref(new Date())
const months = [
    'January', 
    'February', 
    'March', 
    'April', 
    'May', 
    'June', 
    'July', 
    'August', 
    'September', 
    'October', 
    'November', 
    'December'
]


const overlay = useOverlay();
const modal = overlay.create(EventForm);
const config = ref({
    locale: 'en-us',
    timeRangeSelectedHandling: 'Enabled',
    startDate: `${currentDate.value.getFullYear()}-${String(currentDate.value.getMonth() + 1).padStart(2, '0')}-${String(currentDate.value.getDate()).padStart(2, '0')}`,
    onTimeRangeSelected: async (args) => {
        const calendar = args.control
        calendar.clearSelection()
        navigateTo(`/Day?date=${args.start.toDateLocal().toLocaleDateString()}`)
    },
    eventClickHandling: 'Disabled',
    eventRightClickHandling: 'ContextMenu',
    contextMenu: new DayPilot.Menu({
        items: [
            {
                text: 'Edit',
                onClick:(args) => {
                    let id = args.source.id()
                    $fetch(`/api/v1/Event?id=${id}`,{
                        server: false,
                        onResponse({response})
                        {
                            event.value = response._data
                            modal.open({ eventInfo: event.value})
                        }
                    })
                    console.log(args.source) 
                }
            },
            {
                text: 'Delete', onClick: (args) => {
                    const dp = args.source.calendar
                    dp.events.remove(args.source)
                    $fetch(`/api/v1/Event?id=${args.source.id()}`, {
                        server: false,
                        method: 'DELETE',
                        onResponse({response}) {
                            if(!response.ok)
                            {
                                toast.add({ title: `Could not delete event ${args.source.text()}` })
                            }
                            loadEvents()
                        }  
                    })
                }
            }
        ]
    })
    /* eventDeleteHandling: 'Update',
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
, */
})

const monthRef = ref(null)

const loadEvents = () => {

    let events = []

    let startDate = new Date(currentDate.value)
    startDate.setDate(1)
    startDate.setMonth(startDate.getMonth() - 1)

    let endDate = new Date(currentDate.value)
    startDate.setDate(15)
    endDate.setMonth(endDate.getMonth() + 1)

    // abcdefghijklmnopqrstuvwxyz

    $fetch(`api/v1/Event/Events?start=${startDate.toLocaleDateString()}&end=${endDate.toLocaleDateString()}`,{
        server: false,
        onResponse({response}) {
            for (let event of response._data)
            {
                let eventStart = new Date(event.startAt)
                let eventEnd = new Date(event.endAt)
                events.push({
                    id: event.id,
                    start: eventStart.setHours(eventStart.getHours()-7),
                    end: eventEnd.setHours(eventEnd.getHours()-7),
                    text: event.name,
                    resource: event.roomId
                })
            }
            console.log(events)
            config.value.events = events
        }
    })
}

const onClickPreviousMonth = () => {
    currentDate.value.setMonth(currentDate.value.getMonth() - 1)
    currentDate.value = new Date(currentDate.value)
    currentDate.value = currentDate.value
    config.value.startDate = `${currentDate.value.getFullYear()}-${String(currentDate.value.getMonth() + 1).padStart(2, '0')}-${String(currentDate.value.getDate()).padStart(2, '0')}`
    loadEvents()
}

const onClickNextMonth = () => {
    currentDate.value.setMonth(currentDate.value.getMonth() + 1)
    currentDate.value = new Date(currentDate.value)
    config.value.startDate = `${currentDate.value.getFullYear()}-${String(currentDate.value.getMonth() + 1).padStart(2, '0')}-${String(currentDate.value.getDate()).padStart(2, '0')}`
    loadEvents()
}

onMounted(() => {
    loadEvents()
})
</script>
